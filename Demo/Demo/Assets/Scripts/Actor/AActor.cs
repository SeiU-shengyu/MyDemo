using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AActor : Actor {
    private bool isDead;
    private bool isBleeding;
    private bool isVertigo;
    private bool isSlient;
    public AActorMsg aActorMsg;
    public List<AAcotrBuffer> bufferList = new List<AAcotrBuffer>();
    public Text hp;
    public Text mp;
    public Text ad;
    public Text pa;
    public Text ma;
    public Text pd;
    public Text md;
    public Text sp;
    public BufferMsg testBuffer;
    public Vector3 moveTarget;

    public override void ReUse()
    {
        base.ReUse();
    }
    public override void UnUse()
    {
        base.UnUse();
    }

    // Use this for initialization
    void Start () {
        AssetsManager.Instance.AddAliveActor(this);
	}
	
	// Update is called once per frame
	void Update () {
        hp.text = aActorMsg.hp.ToString();
        mp.text = aActorMsg.magic.ToString();
        pa.text = aActorMsg.physicAtk[1].ToString();
        ma.text = aActorMsg.magicAtk[1].ToString();
        pd.text = aActorMsg.physicDef[1].ToString();
        md.text = aActorMsg.magicDef[1].ToString();
        sp.text = aActorMsg.moveSpeed[1].ToString();
        ad.text = aActorMsg.atkDistance[1].ToString();
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    AAcotrBuffer buffer = AssetsManager.Instance.GetActor(ActorType.AACTORBUFFER) as AAcotrBuffer;
        //    testBuffer.bufferType = Tools.GetValue<BufferType>(
        //    BufferType.BT_ADD_MAXHP,
        //    BufferType.BT_ADD_MAXMAGIC,
        //    BufferType.BT_ADD_PATK,
        //    BufferType.BT_ADD_MATK,
        //    BufferType.BT_ADD_PDEF,
        //    BufferType.BT_ADD_MDEF,
        //    BufferType.BT_ADD_ATKDIS,
        //    BufferType.BT_ADD_MOVESPEED,
        //    BufferType.BT_ADD_HP_CONTINUE,
        //    BufferType.BT_ADD_MAGIC_CONTINUE
        //    );
        //    testBuffer.bufferTime = Random.Range(1, 10);
        //    testBuffer.bufferValue = Random.Range(1, 20);
        //    buffer.SetBufferMsg(this,testBuffer);
        //    AddBuffer(buffer);
        //}
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, 10 * Time.deltaTime);
    }

    public void DamagedBy(AtkInfo atkInfo)
    {
        if (isDead)
            return;
        Debug.Log("Damaged");
        if(atkInfo.buffer != null)
            AddBuffer(atkInfo.buffer);
    }

    private void ChangeHp(int changeValue,bool isChangeMaxHp = false)
    {
        if (isChangeMaxHp)
        {
            aActorMsg.maxHp += changeValue;
            if (changeValue > 0)
                aActorMsg.hp += changeValue;
        }
        else
        {
            aActorMsg.hp += changeValue;
        }
        if (aActorMsg.hp > aActorMsg.maxHp)
            aActorMsg.hp = aActorMsg.maxHp;
        if (aActorMsg.hp <= 0)
            isDead = true;
    }
    private void ChangeMagic(int changeValue, bool isChangeMaxMagic = false)
    {
        if (isChangeMaxMagic)
        {
            aActorMsg.maxMagic += changeValue;
            if (changeValue > 0)
                aActorMsg.magic += changeValue;
        }
        else
        {
            aActorMsg.magic += changeValue;
        }
        if (aActorMsg.magic > aActorMsg.maxMagic)
            aActorMsg.magic = aActorMsg.maxMagic;
        if (aActorMsg.magic < 0)
            aActorMsg.magic = 0;
    }

    public void BufferAffect(BufferMsg buffer)
    {
        switch (buffer.bufferType)
        {
            case BufferType.BT_ADD_MAXHP:
                ChangeHp(buffer.bufferValue, true);
                break;
            case BufferType.BT_ADD_MAXMAGIC:
                ChangeMagic(buffer.bufferValue, true);
                break;
            case BufferType.BT_ADD_PATK:
                aActorMsg.physicAtk[1] += buffer.bufferValue;
                break;
            case BufferType.BT_ADD_MATK:
                aActorMsg.magicAtk[1] += buffer.bufferValue;
                break;
            case BufferType.BT_ADD_PDEF:
                aActorMsg.physicDef[1] += buffer.bufferValue;
                break;
            case BufferType.BT_ADD_MDEF:
                aActorMsg.magicDef[1] += buffer.bufferValue;
                break;
            case BufferType.BT_ADD_ATKDIS:
                aActorMsg.atkDistance[1] += buffer.bufferValue;
                break;
            case BufferType.BT_ADD_MOVESPEED:
                aActorMsg.moveSpeed[1] += buffer.bufferValue;
                break;
            case BufferType.BT_ADD_HP_CONTINUE:
                ChangeHp(buffer.bufferValue);
                break;
            case BufferType.BT_ADD_MAGIC_CONTINUE:
                ChangeMagic(buffer.bufferValue);
                break;
        }
    }

    public void AddBuffer(AAcotrBuffer buffer)
    {
        BufferAffect(buffer.bufferMsg);
        bufferList.Add(buffer);
        buffer.transform.SetParent(transform);
    }

    public void RemoveBuffer(AAcotrBuffer buffer)
    {
        BufferAffect(buffer.bufferMsg);
        bufferList.Remove(buffer);
    }
}
