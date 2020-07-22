using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : Actor {
    public AActor targetAActor;
    public AActor aimAActor;
    public AtkInfo atkInfo;

	// Use this for initialization
	void Start () {
        actorType = ActorType.SKILL;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position == aimAActor.transform.position)
        {
            Debug.Log("Atk");
            atkInfo.buffer.gameObject.SetActive(true);
            aimAActor.DamagedBy(atkInfo);
            Release();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, aimAActor.transform.position, 10 * Time.deltaTime);
        }
	}

    public void SetMsg(AActor targetAActor, AActor aimAActor, AtkInfo atkInfo)
    {
        this.targetAActor = targetAActor;
        this.aimAActor = aimAActor;
        this.atkInfo = atkInfo;
    }
}
