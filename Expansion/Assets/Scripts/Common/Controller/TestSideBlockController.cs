using Assets.Scripts.Common.View;
using Assets.Scripts.World.Controller;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common.Controller
{
    public class TestSideBlockController : ILifecycleEventAware
    {
        protected List<ILifecycleEventAware> lifecycleEventAwares = new List<ILifecycleEventAware>();

        private GameObject gameObject;
        private Rigidbody2D entityRb;

        public TestSideBlockController(Transform parent, Vector3 position, InputController inputController, LayerMask? groundMask = null)
        {
            gameObject = new GameObject();
            gameObject.layer = Constants.GROUND_LAYER_ID;
            gameObject.transform.parent = parent;
            //gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0);
            gameObject.transform.position = position;
            entityRb = gameObject.AddComponent<Rigidbody2D>();
            entityRb.isKinematic = true;
            var blockCollider = gameObject.AddComponent<BoxCollider2D>();
            var blockView = new TestSideBlockView(gameObject);

            lifecycleEventAwares.Add(blockView);

            //inputController.PlayerControls.World.MouseLeftButtonClick.started += (CallbackContext ctx) => { bumpBlock(); };
        }

        public void bumpBlock()
        {
            entityRb.AddForce(new Vector2(-100f, 0f));
            entityRb.isKinematic = false;
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
        }
    }
}
