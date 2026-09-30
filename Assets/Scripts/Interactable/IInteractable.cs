using UnityEngine;

public interface IInteractable
{
    public abstract void OnInteract();
    public abstract GameObject OnEnterRadius();
    public abstract GameObject OnExitRadius();
}
