﻿using System.Numerics;

namespace cmArcade.Shared.Invaders
{
    public class PlayerShip : IGameObject
    {
        public Vector2 pos { get; set; }
        public int healthPoints { get; set; }
        public GraphicAsset model { get; set; }
        public int spriteSelect { get; set; }
        public Direction movingDir { get; set; }
        public float accel { get; set; }

        public bool canShoot { get; set; }
        public List<GraphicAsset>? decals { get; set; } = null;

        public PlayerShip(int row, int col)
        {
            pos = new Vector2(col, row);
            model = ShipModel.playerShip;
            movingDir = Direction.Zero;
            canShoot = true;
            healthPoints = 3;
            accel = 0;
            spriteSelect = 0;
        }

        public bool updatePosition((int row, int col) limits)
        {
            if (pos.X >= 0 && pos.X <= limits.col - model.width - 1)
                pos += new Vector2(pos.X + accel, pos.Y);
            else if (pos.X < 0)
                pos = new Vector2(1, pos.Y);
            else
                pos = new Vector2(limits.col - model.width - 2, pos.Y);

            if (movingDir == Direction.Right)
                accel = 6;
            else if (movingDir == Direction.Left)
                accel = -6;
            else
                accel = accel > 0 ? (accel - 0.5f) : (accel + 0.5f);

            return true;
        }

        public async Task shotTimeout()
        {
            canShoot = false;
            spriteSelect = 1;
            await Task.Delay(250);
            spriteSelect = 0;
            await Task.Delay(250);
            canShoot = true;
        }
    }

}
