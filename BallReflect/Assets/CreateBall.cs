using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBall : MonoBehaviour {
    public GameObject ball;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateBall()
    {
        Instantiate(ball);
    }
}
