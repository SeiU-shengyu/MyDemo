using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrids : UIActor {

    private Transform[] m_itemGrids;

    void Awake()
    {
        m_itemGrids = gameObject.GetComponentsInChildren<Transform>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
