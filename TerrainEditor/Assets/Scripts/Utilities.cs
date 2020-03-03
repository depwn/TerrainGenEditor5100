using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    //fractal brownian motion
    //many Perlin Noise Curves added togther where each successive curve is smaller
    //octaves= number of Perlin Noise Curves used
    //persistance = the change between a curve and its next

    public static float fBM(float x, float y, int oct, float persistance)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        for (int i=0; i<oct; i++)
        {
            total += Mathf.PerlinNoise(x  * frequency, y * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistance;
            frequency *= 2;
        }
        return total / maxValue;
    }

}
