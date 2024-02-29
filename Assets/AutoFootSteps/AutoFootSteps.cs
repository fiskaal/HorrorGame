using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AutoFootSteps : MonoBehaviour
{
    public float Volume = .33f;
    /// <summary>
    /// Min & Max volume Variance
    /// </summary>
    public Vector2 RandomizedVolumeRange = new Vector2(.95f, 1.05f);
    /// <summary>
    /// Min and Max Pitch Variance
    /// </summary>
    public Vector2 RandomizedPitchRange = new Vector2(.95f, 1.05f);

    /// <summary>
    /// The Distance the floor is expected to be down at, from this component.
    /// </summary>
    public float FloorDistance = .55f;


    /// <summary>
    /// The distance required to travel before playing a step.
    /// </summary>
    public float DistancePerStep = 1.7f;

    private AudioSource _audioSource;
    /// <summary>
    /// The sounds that'll be played on step.
    /// </summary>
    public FootProfile CurrentFootProfile;
    /// <summary>
    /// The Type of Material the player is standing on, by name. (Rock, Grass, etc - defined in the supplied FootProfile.)
    /// </summary>
    [HideInInspector] public string CurrentFootMaterialName;
    [HideInInspector] public TerrainCollider CurrentTerrain;
    [HideInInspector] public float[,,] CurrentTerrainAlphas;

    /// <summary>
    /// The speed that the player currently crouches at.
    /// </summary>
    public float CrouchSpeed = 2.5f;
    float _walkThreshhold => Mathf.Lerp(CrouchSpeed, WalkSpeed, .5f);
    /// <summary>
    /// The speed that the player currently walks at.
    /// </summary>
    public float WalkSpeed = 5;
    float _runThreshhold => Mathf.Lerp(WalkSpeed, RunSpeed, .5f);
    /// <summary>
    /// The speed that the player currently runs at.
    /// </summary>
    public float RunSpeed = 10;
    
    void Start() => _audioSource = GetComponent<AudioSource>();
    
    private Vector3[] _previousPositions = new Vector3[6];
    private void insertPosition(Vector3 newPosition)
    {
        for (int i = 5; i >= 1; i--)
            _previousPositions[i] = _previousPositions[i-1];
        _previousPositions[0] = newPosition;
    }

    private float _timeSinceLastStep;
    private float _preStepTravelledDistance;
    void LateUpdate()
    {
        // Only run every .125s
        _timeSinceLastStep += Time.deltaTime;
        if (_timeSinceLastStep < .125f)
            return;
        _timeSinceLastStep = 0;

        // add the distance travelled
        float startY = _previousPositions[0].y;
        _previousPositions[0].y = _previousPositions[1].y;
        _preStepTravelledDistance += Vector3.Distance(_previousPositions[0], _previousPositions[1]);
        _previousPositions[0].y = startY;
        insertPosition(transform.position);

        Vector3 _currentVelocity = (_previousPositions[0] - _previousPositions[1]) / .125f;

        FootMaterialSpec curSpec = null;

        // Jumping
        if (_currentVelocity.y > DistancePerStep / 10 && CurrentFootMaterialName != "Air")
        {
            string preName = CurrentFootMaterialName;
            refreshMaterial();
            curSpec ??= CurrentFootProfile.MaterialSpecifications.FirstOrDefault(spec => spec.MaterialName == preName);
            if (CurrentFootMaterialName == "Air")
                if (curSpec != null)
                    playRandomOrdered(curSpec.Jumps);
                else if (preName == "Terrain")
                    playTerrainRandomOrdered(TerrainAction.Jump);
                
        }
        // Landing
        if (_currentVelocity.y < DistancePerStep / -5 && CurrentFootMaterialName == "Air")
        {
            refreshMaterial();
            curSpec ??= CurrentFootProfile.MaterialSpecifications.FirstOrDefault(spec => spec.MaterialName == CurrentFootMaterialName);
            if (CurrentFootMaterialName != "Air")
                if (curSpec != null)
                    playRandomOrdered(curSpec.Lands);
                else if (CurrentFootMaterialName == "Terrain")
                    playTerrainRandomOrdered(TerrainAction.Land);
        }
        
        // Footsteps
        if (_preStepTravelledDistance > DistancePerStep)
        {
            _preStepTravelledDistance = 0;
            refreshMaterial();
    
            curSpec ??= CurrentFootProfile.MaterialSpecifications.FirstOrDefault(spec => spec.MaterialName == CurrentFootMaterialName);
            if (curSpec != null)
            {
                float randomize = Random.Range(RandomizedVolumeRange.x, RandomizedVolumeRange.y);
                _audioSource.volume = Volume * randomize * curSpec.VolumeMultiplier;
                if (_currentVelocity.magnitude < _walkThreshhold)
                {
                    _audioSource.volume *= .80f;
                    playRandomOrdered(curSpec.SoftSteps);
                }
                else if (_currentVelocity.magnitude < _runThreshhold)
                {
                    playRandomOrdered(curSpec.MediumSteps);
                }
                else
                {
                    playRandomOrdered(curSpec.HardSteps);
                }
            } else if (CurrentFootMaterialName == "Terrain")
            {
                float randomize = Random.Range(RandomizedVolumeRange.x, RandomizedVolumeRange.y);
                _audioSource.volume = Volume * randomize;
                if (_currentVelocity.magnitude < _walkThreshhold)
                {
                    _audioSource.volume *= .80f;
                    playTerrainRandomOrdered(TerrainAction.Crouch);
                }
                else if (_currentVelocity.magnitude < _runThreshhold)
                {
                    playTerrainRandomOrdered(TerrainAction.Walk);
                }
                else
                {
                    playTerrainRandomOrdered(TerrainAction.Run);
                }
            }
        }

        // Skid
        if (_currentVelocity.magnitude < _walkThreshhold && Vector3.Distance(_previousPositions[4], _previousPositions[5]) / .125f > _runThreshhold)
        {
            curSpec ??= CurrentFootProfile.MaterialSpecifications.FirstOrDefault(spec => spec.MaterialName == CurrentFootMaterialName);
            if (curSpec != null )
                playRandomOrdered(curSpec.Scuffs);
            else if (CurrentFootMaterialName == "Terrain")
                playTerrainRandomOrdered(TerrainAction.Slip);
        }
    }

    private Dictionary<Collider, string> _knownColliders = new Dictionary<Collider, string>();
    private Dictionary<Renderer, string> _knownRenderers = new Dictionary<Renderer, string>();
    private Dictionary<Material, string> _knownMaterials = new Dictionary<Material, string>();
    private void refreshMaterial()
    {
        
        if (!Physics.Raycast(new Ray() { origin = transform.position, direction = Vector3.down }, out RaycastHit hitInfo) || hitInfo.distance > FloorDistance)
        {
            CurrentFootMaterialName = "Air";
            return;
        }
        if (_knownColliders.TryGetValue(hitInfo.collider, out string matName))
        {
            CurrentFootMaterialName = matName;
            return;
        } else
        {
            if (hitInfo.collider is TerrainCollider terrainCol)
            {
                CurrentFootMaterialName = "Terrain";
                Vector3 terrainPosition = hitInfo.point - terrainCol.transform.position;
                Vector3 splatMapPosition = new Vector3(
                    terrainPosition.x / terrainCol.terrainData.size.x,
                    0,
                    terrainPosition.z / terrainCol.terrainData.size.z
                );

                int x = Mathf.FloorToInt(splatMapPosition.x * terrainCol.terrainData.alphamapWidth);
                int z = Mathf.FloorToInt(splatMapPosition.z * terrainCol.terrainData.alphamapHeight);

                CurrentTerrain = terrainCol;
                CurrentTerrainAlphas = terrainCol.terrainData.GetAlphamaps(x, z, 1, 1);
                return;
            }
            CurrentFootMaterialName = colliderToMaterial(hitInfo.collider, hitInfo.triangleIndex * 3);
            if (!(hitInfo.collider is MeshCollider meshCol && !meshCol.convex && meshCol.sharedMesh.subMeshCount > 1))
                _knownColliders.Add(hitInfo.collider, CurrentFootMaterialName);
        }
    }
    private string colliderToMaterial(Collider collider, int triIndex)
    {
        FootMaterialTag tag = collider.GetComponent<FootMaterialTag>();
        if (tag != null)
            return tag.MaterialName;

        if (collider is MeshCollider meshCol && !meshCol.convex)
        {
            Renderer renderer = collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                int[] hitTriangle = new int[]
                {
                    meshCol.sharedMesh.triangles[triIndex],
                    meshCol.sharedMesh.triangles[triIndex + 1],
                    meshCol.sharedMesh.triangles[triIndex + 2]
                };
                Material foundMat = null;
                for (int i = 0; i < meshCol.sharedMesh.subMeshCount; i++)
                {
                    int[] subMeshTris = meshCol.sharedMesh.GetTriangles(i);
                    for (int j = 0; j < subMeshTris.Length; j += 3)
                    {
                        if (subMeshTris[j] == hitTriangle[0] &&
                            subMeshTris[j + 1] == hitTriangle[1] &&
                            subMeshTris[j + 2] == hitTriangle[2])
                        {
                            foundMat = renderer.sharedMaterials[i];
                            break;
                        }
                    }
                    if (foundMat != null)
                        break;
                }
                if (foundMat != null)
                {
                    if (_knownMaterials.TryGetValue(foundMat, out string matName))
                        return matName;
                    else
                    {
                        CurrentFootMaterialName = MaterialsToMaterial(new Material[] { foundMat });
                        _knownMaterials.Add(foundMat, CurrentFootMaterialName);
                        return CurrentFootMaterialName;
                    }
                }
            }
        }

        // In cases of manual hitboxes
        Renderer parentRenderer = collider.GetComponentInParent<Renderer>();
        if (parentRenderer == null)
            return null;
        else 
            if (_knownRenderers.TryGetValue(parentRenderer, out string matName))
                return matName;
            else
            {
                CurrentFootMaterialName = MaterialsToMaterial(parentRenderer.sharedMaterials);
                _knownRenderers.Add(parentRenderer, CurrentFootMaterialName);
                return CurrentFootMaterialName;
            }
    }
    private string MaterialsToMaterial(Material[] materials)
    {
        foreach (Material mat in materials)
            foreach (FootMaterialSpec footMat in CurrentFootProfile.MaterialSpecifications)
                foreach (string similarName in footMat.SimilarNames)
                    if (mat.name.Contains(similarName, System.StringComparison.CurrentCultureIgnoreCase))
                        return footMat.MaterialName;

        foreach (Material mat in materials)
            foreach (FootMaterialSpec footMat in CurrentFootProfile.MaterialSpecifications)
                foreach (string texturePropName in mat.GetTexturePropertyNames())
                    foreach (string similarName in footMat.SimilarNames)
                        if (mat.GetTexture(texturePropName)?.name.Contains(similarName, System.StringComparison.CurrentCultureIgnoreCase) ?? false)
                            return footMat.MaterialName;

        return null;
    }

    private uint _stepCount = 0;
    private void playRandomOrdered(AudioClip[] clips, float scaleVol = 1)
    {
        
        if (clips == null || clips.Length == 0)
            return;
        _audioSource.pitch = Random.Range(RandomizedPitchRange.x, RandomizedPitchRange.y);
        _audioSource.PlayOneShot(clips[_stepCount++ % clips.Length], scaleVol);
        if (_stepCount % clips.Length == 0) 
        {
            AudioClip[] shuffled = clips.Randomize().ToArray();
            if (clips[clips.Length - 1] == shuffled[0]) // Prevent sound playing twice edge case
            {
                shuffled[0] = shuffled[shuffled.Length - 1];
                shuffled[shuffled.Length - 1] = clips[clips.Length - 1];
            }
            for (int i = 0; i < clips.Length; i++)
                clips[i] = shuffled[i];
        }
    }
    private enum TerrainAction
    {
        Jump, Land, Walk, Run, Crouch, Slip
    }
    private void playTerrainRandomOrdered(TerrainAction type)
    {
        int i = 0;
        foreach (float weight in CurrentTerrainAlphas)
        {
            if (weight == 0)
            {
                i++;
                continue;
            }
            


            FootMaterialSpec foundSpec = CurrentFootProfile.MaterialSpecifications.FirstOrDefault(
                spec => spec.SimilarNames.Any(simName => CurrentTerrain.terrainData.terrainLayers[i].diffuseTexture.name.Contains(simName, System.StringComparison.CurrentCultureIgnoreCase)));
            
            if (foundSpec == null)
                continue;
            switch (type)
            {
                case TerrainAction.Jump:
                    playRandomOrdered(foundSpec.Jumps, weight * foundSpec.VolumeMultiplier);
                    break;
                case TerrainAction.Land:
                    playRandomOrdered(foundSpec.Lands, weight * foundSpec.VolumeMultiplier);
                    break;
                case TerrainAction.Walk:
                    playRandomOrdered(foundSpec.MediumSteps, weight * foundSpec.VolumeMultiplier);
                    break;
                case TerrainAction.Run:
                    playRandomOrdered(foundSpec.HardSteps, weight * foundSpec.VolumeMultiplier);
                    break;
                case TerrainAction.Crouch:
                    playRandomOrdered(foundSpec.SoftSteps, weight * foundSpec.VolumeMultiplier);
                    break;
                case TerrainAction.Slip:
                    playRandomOrdered(foundSpec.Scuffs, weight * foundSpec.VolumeMultiplier);
                    break;
            }

            i++;
        }
    }
}

public static class AutoFootStepExtensions
{
    private static System.Random rnd = new System.Random();
    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        => source.OrderBy((item) => rnd.Next());
}