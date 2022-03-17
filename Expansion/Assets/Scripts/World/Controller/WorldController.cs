using Assets.Scripts.Common;
using Assets.Scripts.Test.Controller;
using Assets.Scripts.World.Model;
using Assets.Scripts.World.Model.Tile;
using Assets.Scripts.World.View;
using Assets.Scripts.World.WorldGen;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.World.Controller
{
    public class WorldController : ILifecycleEventAware
    {
        //Editor Fields
        public int Key = 1;

        //Private Fields
        private WorldModel world;
        private WorldTileView[,] worldTileViews;
        private HighlightBorderView borderView;
        private System.Random rng;

        private readonly Transform WorldTransform;
        private InputController InputController;

        private List<ILifecycleEventAware> lifecycleEventAwares = new List<ILifecycleEventAware>();

        public WorldController(Transform worldTransform, InputController inputController)
        {
            borderView = new HighlightBorderView();
            lifecycleEventAwares.Add(borderView);

            //TODO: Make settileviewasworldchild take a view interface that it can use to get the transform.
            borderView.BorderGameObject.transform.SetParent(worldTransform);

            WorldTransform = worldTransform;
            InputController = inputController;
            InputController.RegisterOnCursorOverWorldCoordinateChangedCallback(OnCursorOverWorldCoordinateChanged);
            InputController.RegisterOnDoubleClickWorldCoordinateCallback(OnDoubleClickWorldCoorinate);
        }

        public void Awake()
        {
            foreach (var aware in lifecycleEventAwares) aware.Awake();
        }

        public void Start()
        {
            foreach (var aware in lifecycleEventAwares) aware.Start();

            rng = new System.Random(Key);

            world = new WorldModel(100, 100);

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

        public void OnEnable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnEnable();
        }

        public void OnDisable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDisable();
        }

        public void Update()
        {
            foreach (var aware in lifecycleEventAwares) aware.Update();
        }

        public void FixedUpdate()
        {
            foreach (var aware in lifecycleEventAwares) aware.FixedUpdate();
        }

        public void OnDestroy()
        {
            GameStateController.Instance.World = world;
        }

        private WorldTileView CreateWorldTileView(WorldTile tileArg)
        {
            WorldTileView tileView = new WorldTileView(tileArg, rng);

            worldTileViews[tileArg.X, tileArg.Y] = tileView;
            return tileView;
        }

        private void SetTileViewAsWorldChild(WorldTileView view)
        {
            view.TileGameObject.transform.SetParent(WorldTransform);
        }

        private void OnCursorOverWorldCoordinateChanged(int x, int y)
        {
            world.ActiveWorldX = x;
            world.ActiveWorldY = y;
            borderView.SetWorldCoordinates(x, y);
        }

        private void OnDoubleClickWorldCoorinate()
        {
            SceneManager.LoadScene("Test");
        }
    }
}