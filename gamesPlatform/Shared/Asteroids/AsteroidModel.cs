﻿using System.Drawing;
using System.Numerics;

namespace cmArcade.Shared.Asteroids;

public class AsteroidModel : CanvasRenderedVectorial
{
    public override string strokeColor { get; set; }
    public override float strokeLineWidth { get; set; }
    public override IEnumerable<Vector2> points { get; set; }
    public override float objWidth { get; set; }
    public override float objHeight { get; set; }
    public override string? fillColor { get; set; }
    public override Vector2 upRightBounds { get; set; }
    public override Vector2 bottomLeftBounds { get; set; }

    private AsteroidModel()
    {
        strokeColor = "white";
        strokeLineWidth = 2f;
        objWidth = 0;
        objHeight = 0;
    }

    public static AsteroidModel GenerateRandomAsteroid(bool isPrimary)
    {
        var newPoints = new List<Vector2>();

        int pointCount = Random.Shared.Next(6, 10);
        var startingPt = new Vector2(isPrimary ? 20 : 0, Random.Shared.Next(10, isPrimary ? 60 : 20));
        double angleDiv = 6.28 / --pointCount;
        double angle = angleDiv;

        float leftWidth = 1;
        float rightWidth = -1;
        float topHeight = -1;
        float bottomHeight = 1;

        while (pointCount-- > 0)
        {
            //create points by spinning around the initial pos offsetting the next one outwards by random
            double x = startingPt.X * Math.Cos(angle) - startingPt.Y * Math.Sin(angle);
            double y = startingPt.X * Math.Sin(angle) + startingPt.Y * Math.Cos(angle);
            var newPt = new Vector2(
                (float)(x + (x * Random.Shared.NextDouble())),
                (float)(y + (y * Random.Shared.NextDouble())));
            newPoints.Add(newPt);

            if (newPt.X > rightWidth)
                rightWidth = newPt.X;
            else if (newPt.X < leftWidth)
                leftWidth = newPt.X;

            if (newPt.Y < bottomHeight)
                bottomHeight = newPt.Y;
            else if (newPt.Y > topHeight)
                topHeight = newPt.Y;

            angle += angleDiv;
        }
        return new AsteroidModel() {
            points = newPoints,
            objWidth = rightWidth + Math.Abs(leftWidth),
            objHeight = topHeight + Math.Abs(bottomHeight),
            upRightBounds = new Vector2(rightWidth, topHeight),
            bottomLeftBounds = new Vector2(leftWidth, bottomHeight),
            fillColor = isPrimary
                ? (Random.Shared.NextDouble() > 0.5 ? "#81858955" : "#A9A9A999")
                : "#E5E4E2EE"
        };
    }
}
