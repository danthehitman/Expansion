using Assets.Scripts.Common.Manager;
using UnityEngine;

namespace Assets.Scripts.Common.View
{
    public class TestSideBlockView : IAmAView, ILifecycleEventAware
    {
        public GameObject GameObject { get; set; }

        public TestSideBlockView(GameObject gameObject)
        {
            GameObject = gameObject;
            var playerSr = gameObject.AddComponent<SpriteRenderer>();
            playerSr.sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_BOREAL_FOREST_SPRITE);
        }

        public void Awake()
        {
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }

        public void OnEnable()
        {
        }

        public void OnDisable()
        {
        }

        public void OnDestroy()
        {
        }
    }
}
