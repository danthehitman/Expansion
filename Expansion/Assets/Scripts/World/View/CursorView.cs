using Assets.Scripts.Common.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.World.View
{
    public class CursorView
    {
        public GameObject GameObject { get; set; }

        public CursorView(Transform parent)
        {
            GameObject = new GameObject(Constants.CURSOR_SPRITE, typeof(RectTransform));
            GameObject.layer = Constants.UI_LAYER_ID;
            GameObject.SetActive(false);
            ((RectTransform)GameObject.transform).sizeDelta = new Vector2(64, 64);
            GameObject.AddComponent<CanvasRenderer>();
            var cursorImage = GameObject.AddComponent<Image>();
            cursorImage.sprite = SpriteManager.Instance.GetSpriteByName(Constants.CURSOR_SPRITE);
            GameObject.transform.SetParent(parent);
            GameObject.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
    }
}
