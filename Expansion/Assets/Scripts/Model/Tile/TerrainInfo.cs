using Assets.Scripts.Model.Enums;

namespace Assets.Scripts.Model.Tile
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
