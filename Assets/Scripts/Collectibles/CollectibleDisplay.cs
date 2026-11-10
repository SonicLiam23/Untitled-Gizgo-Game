using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CDisplayItem", menuName = "Scriptable Objects/CDisplayItem")]
public class CollectibleDisplay : ScriptableObject
{
    [Serializable]
    public struct Display
    {
        public string name; 
        public string description;
        public GameObject displayObject;
    }

    public Display display;
}
