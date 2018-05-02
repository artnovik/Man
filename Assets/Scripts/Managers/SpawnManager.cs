using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private bool autoSpawner;

    private uint deadBodyDeleteDuration = 5; // Standard value: 5

    public GameObject enemyZombie;
    public Transform[] spawnPointsTransforms;

    public void SpawnEnemies(GameObject enemyPrefab, uint count)
    {
        for (var i = 0; i < count; i++)
            if (spawnPointsTransforms[i] != null)
            {
                Instantiate(enemyPrefab, spawnPointsTransforms[i].position, spawnPointsTransforms[i].rotation);
            }
    }

    public void SetDeadBodyDeleteDuration(uint durationInSeconds)
    {
        deadBodyDeleteDuration = durationInSeconds;
    }

    public uint GetDeadBodyDeleteDuration()
    {
        return deadBodyDeleteDuration;
    }

    public int GetCurrentEnemyQuantityOnMap(GameObject enemyPrefab)
    {
        return GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Count();
    }

    public bool CheckForEnemies(GameObject enemyPrefab)
    {
        return GetCurrentEnemyQuantityOnMap(enemyPrefab) > 1;
    }

    public void SpawnEnemies(GameObject enemyPrefab, float delay, bool message)
    {
        StartCoroutine(SpawnWithMessageIfNoEnemiesRoutine(enemyPrefab, delay, message));
    }

    private IEnumerator SpawnWithMessageIfNoEnemiesRoutine(GameObject enemyPrefab, float delay, bool message)
    {
        yield return new WaitForSeconds(delay);

        if (message)
        {
            UIGamePlay.Instance.DisplayMessage(Messages.messageNoEnemies, Colors.yellowMessage, 3f, false);
            yield return new WaitForSeconds(3f);
        }

        if (autoSpawner)
        {
            AutoSpawn(enemyPrefab);
        }
        else
        {
            SpawnEnemies(enemyPrefab, (uint) spawnPointsTransforms.Length);
        }
    }

    #region Singleton

    public static SpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region AutoSpawner

    private void Start()
    {
        if (autoSpawner)
        {
            GetStartTransforms(enemyZombie);
        }
    }

    private GameObject[] allStartEnemies;
    private readonly List<Vector3> autoSpawnPositions = new List<Vector3>();
    private readonly List<Quaternion> autoSpawnRotations = new List<Quaternion>();

    public void GetStartTransforms(GameObject enemyPrefab)
    {
        allStartEnemies = GameObject.FindGameObjectsWithTag(enemyPrefab.tag);

        foreach (GameObject enemy in allStartEnemies)
        {
            autoSpawnPositions.Add(enemy.transform.position);
            autoSpawnRotations.Add(enemy.transform.rotation);
        }
    }

    public void AutoSpawn(GameObject enemyPrefab)
    {
        for (var i = 0; i < allStartEnemies.Length; i++)
            Instantiate(enemyPrefab, autoSpawnPositions[i], autoSpawnRotations[i]);
    }

    #endregion
}