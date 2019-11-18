using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour {


    public int maxHeight = 150;

    public float smooth = 0.01f, persistence = 0.05f;
    public int octaves = 4;
    public float offsetX, offsetZ, scale = 2f;



    public int GenerateStoneHeight(float x, float z) {
        float height = Map(0, maxHeight - 5, 0, 1, fBM(x * smooth * 2, z * smooth * 2, octaves + 1, persistence));
        return (int)height;
    }

    public  int GenerateHeight(float x, float z) {
        float height = Map(0, maxHeight, 0, 1, fBM(x * smooth, z * smooth, octaves, persistence));
        return (int)height;
    }

    public float fBM3D(float x, float y, float z, float sm, int oct) {
        float XY = fBM(x * sm, y * sm, oct, 0.5f);
        float YZ = fBM(y * sm, z * sm, oct, 0.5f);
        float XZ = fBM(x * sm, z * sm, oct, 0.5f);

        float YX = fBM(y * sm, x * sm, oct, 0.5f);
        float ZY = fBM(z * sm, y * sm, oct, 0.5f);
        float ZX = fBM(z * sm, x * sm, oct, 0.5f);

        return (XY + YZ + XZ + YX + ZY + ZX) / 6.0f;
    }

    static float Map(float newmin, float newmax, float origmin, float origmax, float value) {
        return Mathf.Lerp(newmin, newmax, Mathf.InverseLerp(origmin, origmax, value));
    }

    static float fBM(float x, float z, int oct, float pers) {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        for (int i = 0; i < oct; i++) {
            total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;

            maxValue += amplitude;

            amplitude *= pers;
            frequency *= 2;
        }

        return total / maxValue;
    }


    public int OldGenerateHeight(float x, float z) {

        float modx = x / (World.Instance.chunkSize * World.Instance.initialWorldSize) * scale + offsetX;
        float modz = z / (World.Instance.chunkSize * World.Instance.initialWorldSize) * scale + offsetZ;
        
        return (int)(maxHeight * Mathf.PerlinNoise(modx, modz));

    }
}
