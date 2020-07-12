using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    ITEM_CONSUMABLE,
    ITEM_TASK,
}

public struct ItmeMsg
{
    public ItemType type;
    public int id;
    public int counts;
    public string describe;
    public Image icon;
}

public enum ActorType
{
    ITEM,
}
