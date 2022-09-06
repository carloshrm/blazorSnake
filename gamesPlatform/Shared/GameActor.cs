﻿namespace gamesPlatform.Shared
{
    public abstract class GameActor
    {
        public abstract int row { get; set; }
        public abstract int col { get; set; }
        public abstract int healthPoints { get; set; }
        public abstract ShipModel model { get; set; }
        public abstract void updatePosition(int rowEdge, int colEdge);
    }
}