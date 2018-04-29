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

    public void GenerateContainer(Transform transform, ContainerTypeEnum.Enum containerType)
    {
        var generatedContainer = Instantiate(containerPrefab, transform.position, containerPrefab.transform.rotation,
            CollectiblesManager.Instance.collectiblesParentTransform);

        var generatedList = CreateItemsListInContainer(containerType);

        generatedContainer.GetComponent<Container>().FillContainer(generatedList);
    }

    private List<Item> CreateItemsListInContainer(ContainerTypeEnum.Enum containerType)
    {
        switch (containerType)
        {
            case ContainerTypeEnum.Enum.Zombie:
                return LootGenerator.Instance.GenerateZombieItems();
                break;
            default:
                Debug.Log("Check parameters");
                return null;
                break;
        }
    }
}