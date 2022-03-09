using Assets.Scripts.Model;
using Assets.Scripts.Model.Tile;
using UnityEngine;

namespace Assets.Scripts.WorldGen
{
    public class TestWorldGenerator : WorldGenerator
    {
        public TestWorldGenerator(World world, int seed = -1) : base(world, seed)
        {
        }

        public override void Generate()
        {
            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    var newTile = new WorldTile(x, y);
                    if (Random.Range(0, 2) == 0)
                    {
                        newTile.TerrainInfo = new TerrainInfo(Model.Enums.BiomeType.Grassland);
                    }
                    else
                    {
                        newTile.TerrainInfo = new TerrainInfo(Model.Enums.BiomeType.BorealForest);
                    }
                    WorldTiles[x, y] = newTile;
                }
            }
        }
    }
}
