using System;
using UnityEngine;

// https://docs.unity3d.com/6000.7/Documentation/Manual/script-serialization-dictionaries.html

public enum ItemType
{
    Plant,
    Meat,
    Stick,
    Campfire,
    Tree
}

[CreateAssetMenu(fileName = "ItemsConfig", menuName = "Scriptable Objects/ItemsConfig")]
public class ItemsConfig : ScriptableObject
{
    [Serializable]
    public class ItemEntry
    {
        public ItemType type;
        public GameObject prefab;
    }

    [SerializeField] public ItemEntry[] items;
    // [SerializeField] public Dictionary<ItemType, ItemEntry> itemCounts = new Dictionary<ItemType, ItemEntry>();
    public ItemEntry GetItem(ItemType type)
    {

        foreach (ItemEntry item in items)
        {
            if (type == item.type)
            {
                return item;
            }
        }

        return items[items.Length - 1]; 
    }
}
