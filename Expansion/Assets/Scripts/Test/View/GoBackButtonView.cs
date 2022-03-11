using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Test.View
{
    public class GoBackButtonView
    {
        public GameObject GameObject { get; set; }


        public GoBackButtonView(Transform parent)
        {
            GameObject = new GameObject("GoBackButton");
            GameObject.AddComponent<RectTransform>();
            GameObject.AddComponent<Image>();
            var button = GameObject.AddComponent<Button>();
            var textGameObject = new GameObject("Text");
            textGameObject.transform.SetParent(button.transform);
            textGameObject.AddComponent<RectTransform>();
            var buttonText = textGameObject.AddComponent<Text>();
            buttonText.text = "Go Back Test";
            buttonText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            buttonText.color = Color.black;
            GameObject.transform.position = new Vector3(500, 500, 0);
            GameObject.transform.SetParent(parent.GetChild(0).transform);

            button.onClick.AddListener(() => { OnButtonClick(); });
        }

        private void OnButtonClick()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
