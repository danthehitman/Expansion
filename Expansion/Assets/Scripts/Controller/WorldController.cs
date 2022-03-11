using Assets.Scripts.Model;
using Assets.Scripts.Model.Tile;
using Assets.Scripts.View;
using Assets.Scripts.WorldGen;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class WorldController
    {
        //Editor Fields
        public int Key = 1;

        //Private Fields
        private World world;
        private WorldTileView[,] worldTileViews;
        private HighlightBorderView borderView;
        private System.Random rng;

        private readonly Transform GameTransform;
        private InputController InputController;

        public WorldController(Transform gameTransform, InputController inputController, HighlightBorderView borderView)
        {
            this.borderView = borderView;
            //TODO: Make settileviewasworldchild take a view interface that it can use to get the transform.
            borderView.BorderGameObject.transform.SetParent(gameTransform);

            GameTransform = gameTransform;
            InputController = inputController;
            inputController.RegisterOnCursorOverWorldCoordinateChangedCallback(OnCursorOverWorldCoordinateChanged);
        }

        public void Start()
        {
            borderView.Start();

            rng = new System.Random(Key);

            world = new World(100, 100);

            WorldGenerator generator = new TestWorldGenerator(world, Key);
            generator.Generate();
            world.WorldTiles = generator.WorldTiles;

            worldTileViews = new WorldTileView[world.Height, world.Width];

            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    var tileData = world.GetTileAt(x, y);
                    SetTileViewAsWorldChild(CreateWorldTileView(tileData));
                }
            }
        }

        public void Update()
        {
            borderView.Update();
        }

        private WorldTileView CreateWorldTileView(WorldTile tileArg)
        {
            WorldTileView tileView = new WorldTileView(tileArg, rng);

            worldTileViews[tileArg.X, tileArg.Y] = tileView;
            return tileView;
        }

        private void SetTileViewAsWorldChild(WorldTileView view)
        {
            view.TileGameObject.transform.SetParent(GameTransform);
        }

        private void OnCursorOverWorldCoordinateChanged(int x, int y)
        {
            borderView.SetWorldCoordinates(x, y);
        }
    }
}