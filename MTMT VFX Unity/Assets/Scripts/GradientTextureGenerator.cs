using System.IO;
using UnityEngine;

public class GradientTextureGenerator : MonoBehaviour
{
    public Gradient gradient;
    private string savePath = "/Res/VFX/Gradient/";

    private int width = 256;
    private int height = 1;

    private Texture2D gradientTexture;
    private Texture2D temp;

    public Texture2D GenerateGradientTexture(Gradient grad)
    {
        if (temp == null)
        {
            temp = new Texture2D(width, height);
        }

        for (int x = 0; x < width; ++x)
        {
            Color color = grad.Evaluate((float)x / width);
            for (int y = 0; y < height; ++y)
            {
                temp.SetPixel(x, y, color);
            }
        }

        temp.wrapMode = TextureWrapMode.Clamp;
        temp.Apply();
        return temp;
    }

    public void BakeGradientTexture()
    {
        gradientTexture = GenerateGradientTexture(gradient);
        byte[] bytes = gradientTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + savePath + "GradientTexture_" + UnityEngine.Random.Range(0, 999999).ToString() + ".png", bytes);
    }
}
