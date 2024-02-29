using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Foot Profile", menuName = "Auto Foot Steps/Foot Profile", order = 1)]
public class FootProfile : ScriptableObject
{
    public string ProfileName;
    public FootMaterialSpec[] MaterialSpecifications;
}
[System.Serializable]
public class FootMaterialSpec
{
    public string MaterialName;
    [Range(0, 1)]
    public float VolumeMultiplier = 1;
    public string[] SimilarNames;
    public AudioClip[] SoftSteps;
    public AudioClip[] MediumSteps;
    public AudioClip[] HardSteps;
    public AudioClip[] Scuffs;
    public AudioClip[] Jumps;
    public AudioClip[] Lands;
}