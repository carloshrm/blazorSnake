﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace cmArcade.Shared.Asteroids;

public class AsteroidsField : IGameField
{
    private readonly (int row, int col) limits;
    private readonly Random rng = new Random();

    public int activeEdges { get; private set; }
    public string uiMessage { get; set; } = string.Empty;
    public int scoreMult { get; set; } = 1;

    private readonly int asteroidLimit = 5;
    private readonly int baseScore = 3;

    private PlayerShip player { get; set; }
    public List<Asteroid> asteroids { get; set; }

    public AsteroidsField((int row, int col) limits)
    {
        this.limits = limits;
        lines = new List<Vector2>();
        player = new PlayerShip((limits.row / 2, limits.col / 2));
        asteroids = GenerateField();
    }

    private List<Asteroid> GenerateField()
    {
        var field = new List<Asteroid>();
        var playerPos = player.getParts().Select(p => p.pos);

        int astCount = asteroidLimit;
        while (astCount-- > 0)
        {

            float nextX = 0;
            float nextY = 0;
            do
            {
                nextX = rng.Next(100, limits.col - 100);
                nextY = rng.Next(100, limits.row - 100);
            } while (playerPos.Any(p => p.X == nextX || p.Y == nextY));

            field.Add(new Asteroid(new Vector2(nextX, nextY)));
        }
        return field;
    }
    public List<Vector2> lines { get; set; }
    private void CheckHit()
    {
        if (player.shots.Count == 0) 
            return;

        lines = new List<Vector2>();

        foreach (var a in asteroids)
        {
            Vector2? previousPoint = null;
            foreach (var p in a.model.points)
            {
                if (previousPoint == null)
                    previousPoint = p;
                else
                {
                    int x = (int) (previousPoint.Value.X + a.pos.X);
                    int pointX = (int)(p.X + a.pos.X);
                    int y = (int) (previousPoint.Value.Y + a.pos.Y);
                    int pointY = (int)(p.Y + a.pos.Y);
                    while (x != pointX || y != pointY)
                    {
                        lines.Add(new Vector2(x, y));
                        //Console.WriteLine($"x {x} to {pointX}");
                        //Console.WriteLine($"y {y} to {pointY}");
                        if (x != pointX)
                            x += x > pointX ? -1 : 1;

                        if (y != pointY)
                            y += y > pointY ? -1 : 1;
                    }
                    previousPoint = p;
                }
            }
        }
        foreach (var ps in player.shots)
        {
            foreach (var ln in lines)
            {
                if ((int)ps.pos.X == ln.X && (int)ps.pos.Y == ln.Y)
                    Console.WriteLine("!! - hit");
            }
        }
    }

    public bool checkGameOver()
    {
        return false;
    }

    public object getPlayer()
    {
        return player;
    }

    public void setMessage(string msg)
    {
        // TODO
    }

    public void setScoreMultiplier(int val)
    {
        // TODO
    }

    public void parseKeyDown(string input)
    {
        switch (input)
        {
            case "Space":
            case " ":
                player.Fire();
                break;
            case "ArrowUp":
            case "w":
                player.Thrust();
                break;
            case "ArrowDown":
            case "s":
                player.Thrust(false);
                break;
            case "ArrowLeft":
            case "a":
                player.Rotate();
                break;
            case "ArrowRight":
            case "d":
                player.Rotate(true);
                break;
            default:
                break;
        }
    }

    public void parseKeyUp(string dir)
    {
        switch (dir)
        {
            case "ArrowLeft":
            case "a":
            case "ArrowRight":
            case "d":
                player.StopRotation();
                break;
            default:
                break;
        }
    }

    public void updateGameState(Score s)
    {
        player.updatePosition(limits);
        player.UpdateShots(limits.col, limits.row);
        CheckHit();
        //asteroids.ForEach(a => a.UpdatePosition(limits.col, limits.row));
        // shots
        // hit detection
        // respawn asteroids
    }
}
