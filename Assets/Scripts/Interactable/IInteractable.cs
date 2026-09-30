using UnityEngine;

public interface IInteractable
{
    public abstract void OnInteract(GameObject interacter);
    public abstract GameObject OnEnterRadius(Collider other);
    public abstract GameObject OnExitRadius(Collider other);
}
