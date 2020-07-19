using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {
    protected ActorType actorType;
    public virtual void ReUse()
    {
        gameObject.SetActive(true);
    }
    public virtual void UnUse()
    {
        gameObject.SetActive(false);
    }
    public virtual void Release()
    {
        AssetsManager.Instance.ReleaseActor(actorType, this);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
