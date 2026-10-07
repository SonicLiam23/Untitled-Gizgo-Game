using System;
using UnityEngine;


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
