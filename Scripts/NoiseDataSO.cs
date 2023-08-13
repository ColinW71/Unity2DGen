using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This scriptable object holds settings for generating noise
[CreateAssetMenu(fileName = "NoiseSettings", menuName = "Data/Noise Settings")]
public class NoiseDataSO : ScriptableObject
{
    [Range(0.01f, 0.9f)] // A slider in the Inspector for adjusting this value between 0.01 and 0.9
    public float startFrequency = 0.1f; // Initial frequency for noise generation

    [Min(1)] // Restricts the value to be greater than or equal to 1
    public int octaves = 3; // Number of octaves for generating noise

    [Min(0)] // Restricts the value to be greater than or equal to 0
    public float persistance = 0.5f; // Persistence value for controlling amplitude decrease in each octave

    [Min(0)] // Restricts the value to be greater than or equal to 0
    public float frequencyModifier = 1.2f; // Frequency modifier for adjusting frequency in each octave

    public Vector2Int offset = new Vector2Int(1000, 0); // Offset for the noise generation

    public int noiseRangeMin = 0, noiseRangeMax = 20; // Range of generated noise values
}
