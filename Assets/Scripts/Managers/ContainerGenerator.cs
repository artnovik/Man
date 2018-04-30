using System.Collections.Generic;
using UnityEngine;

public class ContainerGenerator : MonoBehaviour
{
    #region Singleton

    public static ContainerGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] private GameObject containerPrefab;
    [SerializeField] public Transform containersParentTransform;

    public void GenerateContainer(Transform containerTransform, ContainerTypeEnum.Enum containerType)
    {
        var generatedContainer = Instantiate(containerPrefab, containerTransform.position,
            containerPrefab.transform.rotation, containersParentTransform);

        var generatedList = CreateItemsListInContainer(containerType);

        generatedContainer.GetComponent<Container>().FillContainer(generatedList);
    }

    private List<Item> CreateItemsListInContainer(ContainerTypeEnum.Enum containerType)
    {
        switch (containerType)
        {
            case ContainerTypeEnum.Enum.Zombie:
                return LootGenerator.Instance.GenerateZombieItems();
            default:
                Debug.Log("Check parameters");
                return null;
        }
    }
}