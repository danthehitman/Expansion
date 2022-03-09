using Assets.Scripts.Manager;
using Assets.Scripts.Model.Enums;
using Assets.Scripts.Model.Tile;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class WorldTileView
    {
        public GameObject TileGameObject { get; set; }
        public WorldTile WorldTile { get; set; }


        private System.Random rng;

        public WorldTileView(WorldTile worldTile, System.Random rand)
        {
            rng = rand;
            WorldTile = worldTile;

            TileGameObject = new GameObject();
            TileGameObject.transform.position = new Vector3(WorldTile.X, WorldTile.Y, 0);
            var tile_sr = TileGameObject.AddComponent<SpriteRenderer>();
            tile_sr.sprite = GetTileSprite(worldTile);
            TileGameObject.name = $"{tile_sr.sprite.name}_{worldTile.X}_{worldTile.Y}";

            worldTile.PropertyChanged += OnTileModelDataChanged;
        }

        public virtual void OnTileModelDataChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public static Sprite GetTileSprite(WorldTile tile)
        {
            BiomeType value = tile.TerrainInfo.BiomeType;
            Sprite sprite = null;
            switch (value)
            {
                case BiomeType.BorealForest:
                    sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_BOREAL_FOREST);
                    break;
                case BiomeType.Grassland:
                    sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_GRASSLAND);
                    break;
            }

            return sprite;
        }
    }
}
