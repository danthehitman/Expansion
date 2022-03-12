using Assets.Scripts.Common;
using Assets.Scripts.Common.Manager;
using UnityEngine;

namespace Assets.Scripts.World.View
{
    public class HighlightBorderView : ILifecycleEventAware
    {
        public GameObject BorderGameObject { get; set; }
        private Sprite[] borderSprites;
        private int borderSpriteLength = 18;
        private int lastFrame = 0;
        private float lastFrameDelta = 0;
        private SpriteRenderer borderSpriteRenderer;


        public HighlightBorderView()
        {
            borderSprites = new Sprite[borderSpriteLength];

            BorderGameObject = new GameObject();
            for (int i = 0; i < 18; i++)
            {
                borderSprites[i] = SpriteManager.Instance.GetSpriteByName($"{Constants.BORDER_SPRITE_ROOT} ({i})");
            }
            BorderGameObject.transform.position = new Vector3(0, 0, 0);
            borderSpriteRenderer = BorderGameObject.AddComponent<SpriteRenderer>();
            borderSpriteRenderer.sortingLayerName = Constants.WORLD_UI_SORTING_LAYER_NAME;
            borderSpriteRenderer.sprite = borderSprites[0];
            BorderGameObject.name = "TileBorderHighlight";
        }

        public void Awake()
        {
        }

        public void Start()
        {
        }

        public void OnEnable()
        {
        }

        public void OnDisable()
        {
        }

        public void Update()
        {
            lastFrameDelta += Time.deltaTime;
            if (lastFrameDelta > 0.1f)
            {
                lastFrame = lastFrame < 17 ? lastFrame + 1 : 0;
                borderSpriteRenderer.sprite = borderSprites[lastFrame];
                lastFrameDelta = 0;
            }
        }

        public void OnDestroy()
        {
        }

        public void SetWorldCoordinates(int x, int y)
        {
            BorderGameObject.transform.position = new Vector3(x, y, 0);
        }
    }
}
