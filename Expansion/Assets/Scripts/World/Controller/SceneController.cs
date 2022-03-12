using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World.Controller
{
    public class SceneController : MonoBehaviour
    {
        private List<ILifecycleEventAware> lifecycleEventAwares = new List<ILifecycleEventAware>();


        private PlayerControls playerControls;


        public void Awake()
        {
            //Initialize game
            playerControls = new PlayerControls();

            lifecycleEventAwares.Add(new WorldController(transform, new InputController(transform.GetChild(0), playerControls)));

        }

        public void OnEnable()
        {
            //TODO: Create an interface for non mono game components and add them all
            //to a list and call all of these methods in a loop for each of them.
            playerControls?.Enable();
            foreach (var aware in lifecycleEventAwares) aware.OnEnable();
        }

        public void OnDisable()
        {
            playerControls?.Disable();
            foreach (var aware in lifecycleEventAwares) aware.OnDisable();
        }

        void Start()
        {
            foreach (var aware in lifecycleEventAwares) aware.Start();
        }

        void Update()
        {
            foreach (var aware in lifecycleEventAwares) aware.Update();
        }

        void OnDestroy()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDestroy();
        }
    }
}
