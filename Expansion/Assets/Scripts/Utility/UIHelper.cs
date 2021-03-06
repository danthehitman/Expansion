using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper
{
    public static GameObject GetRectImageGameObject(int width, int height, Color? color, string name, Transform parent = null)
    {
        var go = new GameObject();
        go.name = name;
        if (parent != null)
        {
            go.transform.parent = parent;
            go.transform.position = parent.position;
        }
        var rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, height);
        if (color != null)
        {
            AddSolidColorImageToGameObject(go, width, height, color.Value);
        }
        return go;
    }

    public static GameObject GetRectTextGameObject(int width, int height, Color fontColor, string text, string name, Transform parent = null)
    {
        var go = new GameObject();
        go.name = name;
        if (parent != null)
        {
            go.transform.parent = parent;
            go.transform.position = parent.position;
        }
        var rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, height);
        var display = go.AddComponent<Text>();
        display.horizontalOverflow = HorizontalWrapMode.Overflow;
        display.alignment = TextAnchor.MiddleLeft;
        display.text = text;
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        display.font = ArialFont;
        display.color = fontColor;
        display.resizeTextForBestFit = true;
        return go;
    }

    public static Image AddSolidColorImageToGameObject(GameObject go, int width, int height, Color color)
    {
        var image = go.AddComponent<Image>();
        image.sprite = Sprite.Create(UIHelper.GetBackgroundTexture(width, height, color),
            new Rect(0, 0, width, height), new Vector2(0, 0f), 1f);
        return image;
    }

    public static Texture2D GetBackgroundTexture(int width, int height, Color color)
    {
        var texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        var pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                pixels[x + y * width] = color;
            }
        }

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }

    public static GameObject CreateButton(string name, Transform parent, string text, Action click, int width, int height)
    {
        GameObject button = new GameObject();
        button.name = name;
        button.transform.parent = parent;
        button.AddComponent<RectTransform>();
        button.AddComponent<Button>();
        button.transform.position = parent.position;
        button.GetComponent<Button>().onClick.AddListener(() =>{ click(); });
        var display = button.AddComponent<Text>();
        var rect = button.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, height);
        display.horizontalOverflow = HorizontalWrapMode.Overflow;
        display.alignment = TextAnchor.MiddleLeft;
        display.text = "  " + text;
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        display.font = ArialFont;
        display.color = Color.white;

        return button;
    }
}
