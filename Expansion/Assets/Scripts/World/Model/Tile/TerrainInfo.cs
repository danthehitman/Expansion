using Assets.Scripts.World.Model.Enums;

namespace Assets.Scripts.World.Model.Tile
{
    public class TerrainInfo
    {
        public BiomeType BiomeType { get; set; }

        public TerrainInfo(BiomeType biomeType)
        {
            BiomeType = biomeType;
        }
    }
}
