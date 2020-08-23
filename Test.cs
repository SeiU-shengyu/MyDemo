using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    [SerializeField] private int initLength;
    [SerializeField] private float offset;
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefab;
    private Ball[][] balls;
    private Ball originBall;

    // Use this for initialization
    void Start()
    {
        balls = new Ball[initLength * 2 + 1][];
        for (int i = 0; i < initLength * 2 + 1; i++)
        {
            int counts = initLength * 2 + 1 - Mathf.Abs(i - initLength);
            if (i % 2 == initLength % 2)
            {
                balls[i] = new Ball[initLength * 2 + 1];
                int initIndex = (initLength * 2 + 1 - counts) / 2;
                for (int j = initIndex; j < initIndex + counts; j++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    obj.transform.position = new Vector3((j - initLength) * (offset + radius),
                                                         (i - initLength) * (offset + radius) * Mathf.Sin(60 * Mathf.Deg2Rad),
                                                         0);
                    balls[i][j] = obj.GetComponent<Ball>();
                    balls[i][j].indexX = i;
                    balls[i][j].indexY = j;
                    balls[i][j].type = Random.Range(0, 3);
                    if (i == initLength && j == initLength)
                    {
                        originBall = balls[i][j];
                        Renderer renderer = obj.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color", Color.black);
                    }
                    else
                    {
                        Renderer renderer = obj.GetComponent<Renderer>();
                        if (balls[i][j].type == 0)
                        {
                            renderer.material.SetColor("_Color", Color.red);
                        }
                        else if (balls[i][j].type == 1)
                        {
                            renderer.material.SetColor("_Color", Color.yellow);
                        }
                        else
                        {
                            renderer.material.SetColor("_Color", Color.blue);
                        }
                    }
                }
            }
            else
            {
                balls[i] = new Ball[initLength * 2];
                int initIndex = (initLength * 2 - counts) / 2;
                for (int j = initIndex; j < initIndex + counts; j++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    obj.transform.position = new Vector3((j - initLength + 0.5f) * (offset + radius),
                                                         (i - initLength) * (offset + radius) * Mathf.Sin(60 * Mathf.Deg2Rad),
                                                         0);
                    balls[i][j] = obj.GetComponent<Ball>();
                    balls[i][j].indexX = i;
                    balls[i][j].indexY = j;
                    balls[i][j].type = Random.Range(0, 3);
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (balls[i][j].type == 0)
                    {
                        renderer.material.SetColor("_Color", Color.red);
                    }
                    else if (balls[i][j].type == 1)
                    {
                        renderer.material.SetColor("_Color", Color.yellow);
                    }
                    else
                    {
                        renderer.material.SetColor("_Color", Color.blue);
                    }
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        //Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Debug.Log(camera.WorldToScreenPoint(transform.position));
        if (Input.GetKeyDown(KeyCode.Space))
            ClearBall();
    }

    void ClearBall()
    {

    }

    void GenerateBall(int indexX,int indexY)
    {
        if (indexX % 2 == initLength % 2)
        {
        }
        else
        {
        }
    }

    void OnGUI()
    {
        //GUI.color = Color.black;
        //for (int i = 0; i < balls.Length; i++)
        //{
        //    for (int j = 0; j < balls[i].Length; j++)
        //    {
        //        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //        Vector3 pos = camera.WorldToScreenPoint(balls[i][j].transform.position);
        //        GUI.Label(new Rect(new Vector2(pos.x, Screen.height - pos.y), new Vector2(50, 50)), i.ToString() + "," + j.ToString());
        //    }
        //}
        //Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Vector3 pos = camera.WorldToScreenPoint(transform.position);
        //GUI.Label(new Rect(new Vector2(pos.x, Screen.height - pos.y), new Vector2(50, 50)), pos.y.ToString());
    }
}
