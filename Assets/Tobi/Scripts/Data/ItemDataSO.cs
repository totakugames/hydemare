using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item Data")]
public class ItemDataSO : ScriptableObject
{
    [Header("Display")]
    public string itemName;
    public Sprite icon;

    [Header("ItemID")]
    public string itemID;

    [Header("Description")]
    [TextArea(2, 4)]
    public string description;
}
