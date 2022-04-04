using Assets.Scripts.Common.Manager;
using UnityEngine;

namespace Assets.Scripts.Common.View
{
    public class SideEntityView : ILifecycleEventAware
    {
        private GameObject playerGameObject;
        private SpriteRenderer playerSr;

        private Sprite[] walkingSprites;
        private int walkingSpritesLength = 11;
        private int lastFrame = 0;
        private float lastFrameDelta = 0;

        private bool walking = true;

        public SideEntityView(GameObject gameObject)
        {
            walkingSprites = new Sprite[walkingSpritesLength];
            for (int i = 0; i < walkingSpritesLength; i++)
            {
                walkingSprites[i] = SpriteManager.Instance.GetSpriteByName($"{Constants.WALK_SPRITE_ROOT}{i}");
            }

            playerGameObject = gameObject;
            playerSr = playerGameObject.AddComponent<SpriteRenderer>();
            playerSr.sprite = SpriteManager.Instance.GetSpriteByName(Constants.PLAYER_SPRITE);
        }

        public void HorizontalDirectionChanged(int direction)
        {
            if (direction != 0)
                playerSr.flipX = direction < 0;
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

        public void Update()
        {
            if (walking)
            {
                lastFrameDelta += Time.deltaTime;
                if (lastFrameDelta > 0.05f)
                {
                    lastFrame = lastFrame < (walkingSpritesLength - 1) ? lastFrame + 1 : 0;
                    playerSr.sprite = walkingSprites[lastFrame];
                    lastFrameDelta = 0;
                }
            }
            else
            {
                //TODO: Store this instead of getting it every time.
                playerSr.sprite = SpriteManager.Instance.GetSpriteByName(Constants.PLAYER_SPRITE);
            }
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
