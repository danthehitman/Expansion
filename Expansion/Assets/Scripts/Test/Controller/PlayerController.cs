using Assets.Scripts.Common;
using Assets.Scripts.Common.Model;
using Assets.Scripts.Common.View;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Test.Controller
{
    public class PlayerController : ILifecycleEventAware
    {
        private PlayerModel playerModel;
        private SidePlayerView playerView;


        protected List<ILifecycleEventAware> lifecycleEventAwares = new List<ILifecycleEventAware>();

        public PlayerController(Transform parent, PlayerModel playerModel = null)
        {
            if (playerModel == null)
                playerModel = new PlayerModel();
            this.playerModel = playerModel;
            playerView = new SidePlayerView(new GameObject(), this.playerModel);
            lifecycleEventAwares.Add(playerView);
        }

        public void Awake()
        {
            foreach (var aware in lifecycleEventAwares) aware.Awake();
        }

        public void OnDestroy()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDestroy();
        }

        public void OnDisable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDisable();
        }

        public void OnEnable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnEnable();
        }

        public void Start()
        {
            foreach (var aware in lifecycleEventAwares) aware.Start();
        }

        public void Update()
        {
            foreach (var aware in lifecycleEventAwares) aware.Update();
        }

        public void FixedUpdate()
        {
            foreach (var aware in lifecycleEventAwares) aware.FixedUpdate();
        }
    }
}
