﻿namespace cmArcade.Shared
{
    public abstract class GameObject
    {
        public abstract int row { get; set; }
        public abstract int col { get; set; }
        public abstract int healthPoints { get; set; }
        public abstract GameAsset model { get; set; }
        public abstract bool updatePosition(int rowEdge, int colEdge);
    }
}