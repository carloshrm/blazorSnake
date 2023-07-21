﻿
using System.Numerics;

namespace cmArcade.Shared;

public abstract class CanvasRenderedVectorial
{
    public abstract string lnColor { get; init; }
    public abstract float lnWidth { get; init; }
    public abstract IEnumerable<Vector2> points { get; set; }
    public abstract float objWidth { get; init; }
    public abstract float objHeight { get; init; }
}