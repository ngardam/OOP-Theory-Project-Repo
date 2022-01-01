using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

public class PerlinNoise : MonoBehaviour
{

    public int width = 256;
    public int height = 256;

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    [SerializeField] Hexasphere targetHexa;

    private void Update()
    {
        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
                
            }
        }
        //generate a perlin noise map for the texture
        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);

       // Mathf.
    }

    public void ApplyPerlin2DNoiseMap()
    {
        Texture2D texture = GenerateTexture();

        targetHexa.ApplyHeightMap(texture, 0.4f);
    }

    public void ApplyPerlin3DNoiseMap()
    {

    }

    public static float get3DPerlinNoise(Vector3 point)
    {

        return point.x;
    }

}
