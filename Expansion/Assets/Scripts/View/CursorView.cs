using UnityEngine;

namespace Assets.Scripts.View
{
    public class CursorView : MonoBehaviour
    {
        public RectTransform CursorSprite;

        public CursorView()
        {
        }

        public void OnAwake()
        {
            CursorSprite.transform.position = new Vector3(0, 0, 0);
        }


    }
}
