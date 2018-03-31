using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPointsTransforms;

    public GameObject enemyZombie;

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

    public int GetCurrentEnemyQuantityOnMap(GameObject enemyPrefab)
    {
        return GameObject.FindGameObjectsWithTag(enemyPrefab.tag).Count();
    }

    public bool CheckForEnemies(GameObject enemyPrefab)
    {
        return GetCurrentEnemyQuantityOnMap(enemyPrefab) > 0;
    }

    public void SpawnWithMessageIfNoEnemies(GameObject enemyPrefab, float delay)
    {
        StartCoroutine(SpawnWithMessageIfNoEnemiesRoutine(enemyPrefab, delay));
    }

    private IEnumerator SpawnWithMessageIfNoEnemiesRoutine(GameObject enemyPrefab, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!CheckForEnemies(enemyPrefab))
        {
            UIGamePlay.Instance.DisplayMessage(Messages.messageNoEnemies, Color.yellow, 3f, false);
            yield return new WaitForSeconds(3f);
            SpawnEnemies(enemyPrefab, 4);
        }
    }
}
