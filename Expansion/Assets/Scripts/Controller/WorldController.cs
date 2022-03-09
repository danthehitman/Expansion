using Assets.Scripts.Model;
using Assets.Scripts.Model.Tile;
using Assets.Scripts.View;
using Assets.Scripts.WorldGen;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class WorldController
    {
        public static WorldController Instance;

        //Editor Fields
        public int Key = 1;

        //Private Fields
        private World world;
        private WorldTileView[,] worldTileViews;
        private System.Random rng;

        private readonly Transform GameTransform;
        private InputController InputController;

        public WorldController(Transform gameTransform, InputController inputController)
        {

            GameTransform = gameTransform;
            InputController = inputController;
            inputController.RegisterOnCursorOverWorldCoordinateChangedCallback(OnCursorOverWorldCoordinateChanged);
        }

        // Start is called before the first frame update
        public void Start()
        {
            Instance = this;

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

        // Update is called once per frame
        public void Update()
        {

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
            Debug.Log($"New X and Y coords {x}/{y}");
        }
    }
}