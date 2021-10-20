using UnityEngine;

public class Item : MonoBehaviour, IInteractiveItem
{
    [SerializeField] protected ItemData itemData;

    public string GetName()
    {
        return itemData.itemName;
    }

    public string GetDescription()
    {
        return itemData.itemDescription;
    }

    public virtual void Interact()
    {
    }
}