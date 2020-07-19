using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAcotrBuffer : Actor {
    public AActor targetAActor;
    public BufferMsg bufferMsg;
    private float lastAffectTime = 0;

    public override void ReUse()
    {
        base.ReUse();
    }
    public override void UnUse()
    {
        base.UnUse();
    }
    public void SetBufferMsg(AActor targetAActor, BufferMsg bufferMsg)
    {
        this.bufferMsg = bufferMsg;
        this.targetAActor = targetAActor;
        lastAffectTime = bufferMsg.bufferTime;
    }
    void Awake()
    {
        actorType = ActorType.AACTORBUFFER;
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (bufferMsg.bufferTime < 0)
        {
            bufferMsg.ReverseMsg();
            targetAActor.RemoveBuffer(this);
            Release();
        }
        else
        {
            if (bufferMsg.bufferType >= BufferType.BT_CONTINUED && lastAffectTime - bufferMsg.bufferTime >= 1)
            {
                targetAActor.BufferAffect(bufferMsg);
                lastAffectTime = bufferMsg.bufferTime;
            }
            bufferMsg.bufferTime -= Time.deltaTime;
        }
	}
}
