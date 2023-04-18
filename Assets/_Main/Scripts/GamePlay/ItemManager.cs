using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private List<Vector3> gridPositions = new();
    private Dictionary<ItemType, Item[]> spawnedItems = new();
    private float destroyDelay = .2f;

    #region Grid
    [Header("Grid Variables")]
    [SerializeField] private Transform gridCenter;
    [SerializeField] private Vector2 gridSpacing;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 xOffsetRandomness;
    private float RandomXValue => Random.Range(xOffsetRandomness.x, xOffsetRandomness.y);
    [SerializeField] private Vector2 zOffsetRandomness;
    private float RandomZValue => Random.Range(zOffsetRandomness.x, zOffsetRandomness.y);
    [SerializeField] private Vector2 yOffsetRandomness;
    private float RandomYValue => Random.Range(yOffsetRandomness.x, yOffsetRandomness.y);
    [SerializeField] private bool drawGizmos;

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        foreach (var pos in gridPositions)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pos, .1f);
        }
    }

    [ContextMenu("Create Item Grid")]
    private void CreateItemGrid()
    {
        gridPositions = new();
        if (gridSize.x < 1) gridSize.x = 1;
        if (gridSize.y < 1) gridSize.y = 1;

        Vector3 startOffset = new((gridSize.x - 1) * gridSpacing.x * .5f, 0, (gridSize.y - 1) * gridSpacing.y * .5f);
        Vector3 startPos = gridCenter.position - startOffset;

        for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            int column = i % gridSize.x;
            int row = i / gridSize.x;

            Vector3 insPos = startPos + new Vector3(column * gridSpacing.x + RandomXValue, gridCenter.position.y + RandomYValue, row * gridSpacing.y + RandomZValue);
            gridPositions.Add(insPos);
        }
    }

    #endregion
    private float RandomAngle => Random.Range(0, 360);

    public void SpawnIems(StageData stageData)
    {
        if (gridSize.x * gridSize.y < stageData.itemsToSpawn.Length * 3) Debug.LogError("Grid Size is Not Enough");
        CreateItemGrid();
        var clonePosList = new List<Vector3>(gridPositions);
        int count = stageData.itemsToSpawn.Length;
        for (int i = 0; i < count; i++)
        {
            spawnedItems.Add(stageData.itemsToSpawn[i].itemType, new Item[3]);
            for (int j = 0; j < 3; j++)
            {
                int randomIndex = Random.Range(0, clonePosList.Count);
                var pos = clonePosList[randomIndex];
                clonePosList.RemoveAt(randomIndex);
                var item = Instantiate(stageData.itemsToSpawn[i].itemPrefab, pos, Quaternion.Euler(RandomAngle, RandomAngle, RandomAngle));
                spawnedItems[item.itemData.itemType][j] = item;
            }
        }
    }

    public List<Item> GetUnslottedItems(ItemType itemType)
    {
        Item[] items = spawnedItems[itemType];
        List<Item> itemsToReturn = new();
        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].slotted) itemsToReturn.Add(items[i]);
        }
        return itemsToReturn;
    }

    public void DestroyItems(ItemType itemType)
    {
        Item[] items = spawnedItems[itemType];

        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i].gameObject, destroyDelay);
        }

        spawnedItems.Remove(itemType);

        if (spawnedItems.Count == 0) StageController.Instance.SuccessGame();
    }
}
