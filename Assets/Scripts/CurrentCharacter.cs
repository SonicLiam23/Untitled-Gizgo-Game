using UnityEngine;

public class CurrentCharacter
{
    public GameObject gameObject {  get; private set; }
    public CHARACTER type { get; private set; }

    public CurrentCharacter(GameObject obj, CHARACTER type) 
    {
        this.gameObject = obj;
        this.type = type;
    }
}