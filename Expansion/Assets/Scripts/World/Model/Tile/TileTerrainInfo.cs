using Assets.Scripts.World.Model.Enums;

namespace Assets.Scripts.World.Model.Tile
{
    public class TileTerrainInfo
    {
        public BiomeType BiomeType { get; set; }

        public TileTerrainInfo(BiomeType biomeType)
        {
            BiomeType = biomeType;
        }
    }
}
