using Assets.Scripts.Manager;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class HighlightBorderView
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

        public void Start()
        {
        }

        public void Update()
        {
            lastFrameDelta += Time.deltaTime;
            Debug.Log($"lastFrameDelta: {lastFrameDelta}");
            if (lastFrameDelta > 0.1f)
            {
                Debug.Log("Switching Sprite");
                lastFrame = lastFrame < 17 ? lastFrame + 1 : 0;
                borderSpriteRenderer.sprite = borderSprites[lastFrame];
                lastFrameDelta = 0;
            }
        }

        public void SetWorldCoordinates(int x, int y)
        {
            BorderGameObject.transform.position = new Vector3(x, y, 0);
        }
    }
}
