using Assets.Scripts.Common.Manager;
using UnityEngine;

namespace Assets.Scripts.Common.View
{
    public class SideEntityView : ILifecycleEventAware
    {
        private GameObject playerGameObject;

        public SideEntityView(GameObject gameObject)
        {
            playerGameObject = gameObject;
            var playerSr = playerGameObject.AddComponent<SpriteRenderer>();
            playerSr.sprite = SpriteManager.Instance.GetSpriteByName(Constants.PLAYER_SPRITE);
        }

        public void Awake()
        {
        }

        public void OnEnable()
        {
        }

        public void Start()
        {
        }

        //public float jumpForce = 20;
        //public float gravity = -9.81f;
        //float velocity;
        public void Update()
        {
            //velocity += gravity * Time.deltaTime;
            //if (Mouse.current.leftButton.isPressed)
            //{
            //    velocity = jumpForce;
            //}
            //playerGameObject.transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);
        }

        public void FixedUpdate()
        {
        }

        public void OnDestroy()
        {
        }

        public void OnDisable()
        {
        }
    }
}
