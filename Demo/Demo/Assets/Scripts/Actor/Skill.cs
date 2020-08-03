using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : Actor {
    public AActor targetAActor;
    public AActor aimAActor;
    public AtkInfo atkInfo;
    public SkillMsg skillMsg;

	// Use this for initialization
	void Start () {
        actorType = ActorType.SKILL;
	}
	
	// Update is called once per frame
	void Update () {
        if (skillMsg.delayTime > 0)
        {
            skillMsg.delayTime -= Time.deltaTime;
        }
        else
        {
            if (skillMsg.continueTime > 0)
            {
                CheckDamage();
                skillMsg.continueTime -= Time.deltaTime;
            }
            else
            {
                CheckDamage();
                Release();
            }
        }
	}

    protected virtual bool CheckAffectTarget()
    {
        return true;
    }

    protected virtual bool CheckEnd()
    {
        return true;
    }

    private void CheckDamage()
    {
        if (skillMsg.drParam3 == 0)
        {
            foreach (AActor aActor in AssetsManager.Instance.AliveActor)
            {
                Vector3 dir = aActor.transform.position - transform.position;
                //Debug.Log(Mathf.Abs(Vector3.Dot(dir, transform.right)) + ":" + skillMsg.drParam1);
                //Debug.Log(Mathf.Abs(Vector3.Dot(dir, transform.forward)) + ":" + skillMsg.drParam2);
                if (Mathf.Abs(Vector3.Dot(dir, transform.right)) <= skillMsg.drParam1 &&
                    Mathf.Abs(Vector3.Dot(dir, transform.forward)) <= skillMsg.drParam2)
                {
                    atkInfo.buffer = AssetsManager.Instance.GetActor(ActorType.AACTORBUFFER) as AAcotrBuffer;
                    atkInfo.buffer.SetBufferMsg(aActor, new BufferMsg(BufferType.BT_ADD_HP_CONTINUE, -10, 0));
                    aActor.DamagedBy(atkInfo);
                }
            }
        }
        else
        {
            foreach (AActor aActor in AssetsManager.Instance.AliveActor)
            {
                Vector3 dir = aActor.transform.position - transform.position;
                if(dir.magnitude <= skillMsg.drParam1 && Vector3.Angle(dir,transform.forward) <= skillMsg.drParam3)
                    aActor.DamagedBy(atkInfo);
            }
        }
    }

    public void SetMsg(AActor targetAActor, AActor aimAActor, AtkInfo atkInfo, SkillMsg skillMsg)
    {
        this.targetAActor = targetAActor;
        this.aimAActor = aimAActor;
        this.atkInfo = atkInfo;
        this.skillMsg = skillMsg;
    }
}
