using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public WorldRenderer worldRenderer; // Reference to the WorldRenderer component
    public BlockData blockData; // Reference to the BlockData scriptable object

    public NoiseDataSO heightMapNoiseData, stoneNoiseData, perlin2DData; // Scriptable objects for noise data

    public int mapLength = 50; // Length of the map
    public float amplitude = 1; // Amplitude of the noise
    public float frequency = 0.01f; // Frequency of the noise

    public float noiseThresholdMin = 0.45f; // Minimum threshold for perlin noise
    public float noiseThresholdMax = 0.55f; // Maximum threshold for perlin noise

    // Get the noise value at a given position
    private float GetNoiseValue(int x, int y)
    {
        return amplitude * Mathf.PerlinNoise(x * frequency, y * frequency);
    }

    // Generate the map using better noise generation
    public void GenerateMapBetter()
    {
        worldRenderer.ClearGroundTilemap(); // Clear the ground tilemap
        for (int x = -1 * mapLength; x < mapLength; x++)
        {
            var noise = SumNoise(heightMapNoiseData.offset.x + x, 1, heightMapNoiseData); // Sum the noise values
            var noiseInRange = RangeMap(noise, 0, 1, heightMapNoiseData.noiseRangeMin, heightMapNoiseData.noiseRangeMax); // Map the noise to the desired range
            var noiseEndValue = Mathf.FloorToInt(noiseInRange); // Convert noise value to integer

            var noiseStone = SumNoise(stoneNoiseData.offset.x + x, 1, stoneNoiseData); // Sum the stone noise values
            var noiseStoneInRange = RangeMap(noiseStone, 0, 1, stoneNoiseData.noiseRangeMin, stoneNoiseData.noiseRangeMax); // Map the stone noise to the desired range
            var noiseStoneInt = Mathf.FloorToInt(noiseStoneInRange); // Convert stone noise value to integer

            for (int y = 0; y <= noiseEndValue; y++)
            {
                worldRenderer.SetBackgroundTile(x, y, SelectDarkTile(y, noiseEndValue, noiseStoneInt)); // Set the background tile at (x, y)
                var noisePerlin2D = SumNoise(perlin2DData.offset.x + x, y, perlin2DData); // Sum the 2D perlin noise values
                if (y >= perlin2DData.noiseRangeMin && y <= perlin2DData.noiseRangeMax && noisePerlin2D > noiseThresholdMin && noisePerlin2D < noiseThresholdMax)
                {
                    continue; // Skip the ground tile if conditions are met
                }
                worldRenderer.SetGroundTile(x, y, SelectTile(y, noiseEndValue, noiseStoneInt)); // Set the ground tile at (x, y)
            }
        }
    }
    // Sum the noise values based on octaves and settings
    public float SumNoise(int x, int y, NoiseDataSO noiseSettings)
    {
        float amplitude = 1;
        float frequency = noiseSettings.startFrequency;
        float noiseSum = 0;
        float amplitudeSum = 0;
        for (int i = 0; i < noiseSettings.octaves; i++)
        {
            noiseSum += amplitude * Mathf.PerlinNoise(x * frequency, y * frequency);
            amplitudeSum += amplitude;
            amplitude *= noiseSettings.persistance;
            frequency *= noiseSettings.frequencyModifier;

        }
        return noiseSum / amplitudeSum; // Normalize the noise value to range [0-1]
    }

    // Map a value from one range to another
    private float RangeMap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (inputValue - inMin) * (outMax - outMin) / (inMax - inMin);
    }

    // Select the appropriate tile based on the noise and height values
    private TileBase SelectTile(int y, int noiseValue, int stoneHeight)
    {
        if (y >= stoneHeight)
        {
            if (y == noiseValue)
                return blockData.stoneGrass;
            return blockData.stoneTile;
        }
        else if (y == noiseValue)
        {
            return blockData.dirtGrass;
        }
        return blockData.dirtTile;
    }

    // Select the appropriate dark tile based on the noise and height values
    private TileBase SelectDarkTile(int y, int noiseValue, int stoneHeight)
    {
        if (y >= stoneHeight)
        {
            return blockData.stoneDark;
        }
        return blockData.dirtDark;
    }
}