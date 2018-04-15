using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsManager : MonoBehaviour
{
    #region Singleton

    public static EffectsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField]
    private GameObject bloodEffect;

    private void ActivateEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation, float duration)
    {
        effectCoroutine = StartCoroutine(ActivateEffectRoutine(effectPrefab, position, rotation, duration));
    }

    private Coroutine effectCoroutine;

    private IEnumerator ActivateEffectRoutine(GameObject effectPrefab, Vector3 position, Quaternion rotation, float duration)
    {
        var instancedEffect = Instantiate(effectPrefab, position, rotation);
        yield return new WaitForSeconds(duration);
        Destroy(instancedEffect);
    }

    public void ActivateBloodEffect(Transform parentTransform)
    {
        ActivateEffect(bloodEffect, parentTransform.position, parentTransform.rotation, 3f);
    }
}
