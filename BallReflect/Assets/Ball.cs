using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public float speed = 10;
    private Camera mainCamera;
    private Vector3 moveDir;
	// Use this for initialization
	void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        float rate = Random.Range(0.0f, 1.0f);
        moveDir = (mainCamera.transform.right * rate + mainCamera.transform.up * (1 - rate)).normalized;
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 1));
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += moveDir * Time.deltaTime * speed;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        if (screenPos.x <= 0 && Vector3.Dot(moveDir, mainCamera.transform.right) <= 0)
            moveDir = Vector3.Reflect(moveDir, mainCamera.transform.right);
        if (screenPos.x >= Screen.width && Vector3.Dot(moveDir, mainCamera.transform.right) >= 0)
            moveDir = Vector3.Reflect(moveDir, -mainCamera.transform.right);
        if (screenPos.y <= 0 && Vector3.Dot(moveDir, mainCamera.transform.up) <= 0)
            moveDir = Vector3.Reflect(moveDir, mainCamera.transform.up);
        if (screenPos.y >= Screen.height && Vector3.Dot(moveDir, mainCamera.transform.up) >= 0)
            moveDir = Vector3.Reflect(moveDir, -mainCamera.transform.up);
    }
}
