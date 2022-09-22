using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumables
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;

    public List<ItemEffect> efts;

    public bool Use()
    {
        bool isUsed = false;

        foreach(ItemEffect eft in efts)
        {
            if (DataManager.Inst.Player.HP < DataManager.Inst.Player.MaxHP)
            {
                isUsed = eft.ExecuteRole();
            }
            else
            {
                Debug.Log("hp가 최대치 입니다.");
            }
        }

        return isUsed;
    }
}
