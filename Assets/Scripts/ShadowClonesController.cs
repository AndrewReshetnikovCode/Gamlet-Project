using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowClonesController : MonoBehaviour
{
    public int maxClones;
    public GameObject clonePrefab;
    public List<GameObject> activeClones = new();

    public float delayAtFightStart;
    public float baseInterval;
    public float spawnRedius;
    public float spawnCycleInterval;

    public bool permanentSpawnCycle = false;
    void Start()
    {
        GetComponent<CharacterFacade>().onEntersFight += StartCloneSpawning;
        GetComponent<CharacterFacade>().onExitFight += 
            StopAllCoroutines;
    }
    public void DestroyAllClones()
    {
        foreach (var c in activeClones)
        {
            Destroy(c);
        }
        activeClones.Clear();
    }
    public void StartCloneSpawning()
    {    
        StartCoroutine(CloneSpawning());
        if (permanentSpawnCycle)
        {
            StartCoroutine(CloneSpawningGlobalCycle());
        }
    }
    IEnumerator CloneSpawning()
    {
        while (activeClones.Count != maxClones)
        {
            TryToSpawn();
            yield return new WaitForSeconds(delayAtFightStart);
        }
    }
    IEnumerator CloneSpawningGlobalCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCycleInterval);
            StartCoroutine(CloneSpawning());
        }
    }
    void TryToSpawn()
    {
        if (activeClones.Count < maxClones)
        {
            SpawnClone();
        }
    }
    public void SpawnClone()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRedius;
        randomPos.y = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, spawnRedius, LayerMask.GetMask("Default"));
        GameObject newClone = Instantiate(clonePrefab, hit.position, Quaternion.identity); 
        activeClones.Add(newClone);
        newClone.GetComponentInChildren<CharacterFacade>().death.OnDeath.AddListener(OnCloneDeath);
    }
    void OnCloneDeath()
    {
        foreach (var item in activeClones)
        {
            if (item == null)
            {
                activeClones.Remove(item);
                break;
            }
        }
    }
}
