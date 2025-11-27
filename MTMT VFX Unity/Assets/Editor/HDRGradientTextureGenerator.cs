using UnityEngine;
using UnityEditor;

public class HDRGradientTextureGenerator : EditorWindow
{
    Gradient gradient = new Gradient();
    int width = 256;
    int height = 1;
    string fileName = "HDRGradient.exr";

    [MenuItem("Tools/HDR Gradient Texture Generator")]
    public static void ShowWindow()
    {
        GetWindow<HDRGradientTextureGenerator>("HDR Gradient Generator");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("HDR Gradient Generator", EditorStyles.boldLabel);

        gradient = EditorGUILayout.GradientField("Gradient", gradient);
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("Generate HDR Gradient Texture"))
        {
            CreateGradientTexture();
        }
    }

    void CreateGradientTexture()
    {
        // Create a floating-point HDR texture
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBAHalf, false, true);

        for (int x = 0; x < width; x++)
        {
            float t = x / (float)(width - 1);
            Color color = gradient.Evaluate(t);
            // You can boost intensity if you want stronger HDR
            // e.g. color *= 2.0f;
            for (int y = 0; y < height; y++)
                tex.SetPixel(x, y, color);
        }

        tex.Apply();

        // Encode to EXR (HDR format)
        byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);

        string path = EditorUtility.SaveFilePanel("Save HDR Gradient", "Assets", fileName, "exr");
        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
            Debug.Log($"Saved HDR gradient texture to: {path}");
        }

        DestroyImmediate(tex);
    }
}