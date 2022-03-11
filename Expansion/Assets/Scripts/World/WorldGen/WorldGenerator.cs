using Assets.Scripts.World.Model;
using Assets.Scripts.World.Model.Tile;
using UnityEngine;

namespace Assets.Scripts.World.WorldGen
{
    public abstract class WorldGenerator
    {
        private System.Random rng;
        protected WorldModel world;

        public WorldTile[,] WorldTiles { get; set; }

        public WorldGenerator(WorldModel world, int seed = -1)
        {
            WorldTiles = new WorldTile[world.Width, world.Height];

            this.world = world;
            if (seed == -1)
                seed = Random.Range(0, int.MaxValue);

            rng = new System.Random(seed);
        }

        public abstract void Generate();
    }
}
