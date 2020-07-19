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
    AACTORBUFFER,
    SKILL,
}

[System.Serializable]
public struct AActorMsg
{
    public int hp;
    public int maxHp;
    public int magic;
    public int maxMagic;
    public int[] atkDistance;
    public int[] physicAtk;
    public int[] magicAtk;
    public int[] physicDef;
    public int[] magicDef;
    public int[] moveSpeed;

    public AActorMsg(int hp, int magic, int atkDistance, int physicAtk, int magicAtk, int physicDef, int magicDef, int moveSpeed)
    {
        this.hp = maxHp = hp;
        this.magic = maxMagic = magic;
        this.atkDistance = new int[2] { atkDistance , atkDistance };
        this.physicAtk = new int[2] { physicAtk, physicAtk };
        this.magicAtk = new int[2] { magicAtk, magicAtk };
        this.physicDef = new int[2] { physicDef, physicDef };
        this.magicDef = new int[2] { magicDef, magicDef };
        this.moveSpeed = new int[2] { moveSpeed, moveSpeed };
    }
}

[System.Serializable]
public enum BufferType
{
    BT_FIXED = 0,
    BT_ADD_MAXHP,
    BT_ADD_MAXMAGIC,
    BT_ADD_PATK,
    BT_ADD_MATK,
    BT_ADD_PDEF,
    BT_ADD_MDEF,
    BT_ADD_ATKDIS,
    BT_ADD_MOVESPEED,

    BT_STATUS = 100,
    BT_VERTIGO,
    BT_SILENT,
    BT_BLEEDING,
    
    BT_CONTINUED = 200,
    BT_ADD_HP_CONTINUE,
    BT_ADD_MAGIC_CONTINUE,
}

[System.Serializable]
public struct BufferMsg
{
    public BufferType bufferType;
    public int bufferValue;
    public float bufferTime;

    public BufferMsg(BufferType bufferType, int bufferValue, float bufferTime)
    {
        this.bufferType = bufferType;
        this.bufferValue = bufferValue;
        this.bufferTime = bufferTime;
    }

    public void ReverseMsg()
    {
        if (bufferType < BufferType.BT_STATUS)
            bufferValue *= -1;
        else if (bufferType < BufferType.BT_CONTINUED)
            bufferValue *= -1;
        else
            bufferValue = 0;
    }
}

[System.Serializable]
public struct AtkInfo
{
    public AActor targetAActor;
    public AAcotrBuffer buffer;
}

public enum SkillType
{
}
public struct SkillMsg
{
    public int id;
    public int bufferId;
}
