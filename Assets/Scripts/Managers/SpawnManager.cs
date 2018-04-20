using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPointsTransforms;

    public GameObject enemyZombie;

    private uint deadBodyDeleteDuration = 5;

    #region Singleton

    public static SpawnManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void SpawnEnemies(GameObject enemyPrefab, uint count)
    {
        for (int i = 0; i < count; i++)
        {
            if (spawnPointsTransforms[i] != null)
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
        return GetCurrentEnemyQuantityOnMap(enemyPrefab) > 0;
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

        SpawnEnemies(enemyPrefab, 4);
    }
}
