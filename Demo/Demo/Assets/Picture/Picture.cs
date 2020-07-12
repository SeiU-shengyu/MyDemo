using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour {
    public float speed;
    private float x;
    private float y;
    private float z;
    private bool isStart;
    private Animator animator;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        x = transform.rotation.eulerAngles.x;
        y = transform.rotation.eulerAngles.y;
        z = transform.rotation.eulerAngles.z;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.enabled = true;
            isStart = true;
        }
        if (isStart)
        {
            x += Time.deltaTime * speed;
            y += Time.deltaTime * speed;
            //z += Time.deltaTime * speed;
            transform.rotation = Quaternion.Euler(x, y, z);
        }
	}
}
