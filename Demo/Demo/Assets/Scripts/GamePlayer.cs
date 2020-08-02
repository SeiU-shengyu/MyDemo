using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public AActor myAActor;
    public SkillMsg skillMsg;
    public float cameraMoveSpeed = 5;
    public SpriteRenderer distanceCircle;
    public SpriteRenderer skillCircle;
    public SpriteRenderer skillSquare;

    private Camera mainCamera;
    private int maskLayer;
    private Vector3 floorMousePos;
	// Use this for initialization
	void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        maskLayer = LayerMask.GetMask("OtherPlayer", "Master", "Floor");
    }
	
	// Update is called once per frame
	void Update () {
        CameraCtrl();
        ReleaseSkillTest();
        //if (Input.GetMouseButtonDown(1))
        {
            Vector3 viewPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            viewPos += new Vector3(0, 0, 1);
            Vector3 worldPos = mainCamera.ViewportToWorldPoint(viewPos);
            Vector3 dir = (worldPos - mainCamera.transform.position).normalized;
            RaycastHit hitInfo;
            if (Physics.Raycast(mainCamera.transform.position, dir, out hitInfo, Mathf.Infinity, maskLayer))
            {
                if (hitInfo.transform.gameObject.layer == 10)
                {
                    if (Input.GetMouseButtonDown(1))
                        myAActor.moveTarget = hitInfo.point;
                    floorMousePos = hitInfo.point;
                }
                //else
                //    AtkTest(ref hitInfo);
            }
        }
    }

    private void AtkTest(ref RaycastHit hitInfo)
    {
        AActor target = hitInfo.transform.GetComponent<AActor>();
        if (target == myAActor)
            return;
        Skill skill = AssetsManager.Instance.GetActor(ActorType.SKILL) as Skill;
        skill.transform.position = myAActor.transform.position;
        skill.aimAActor = target;
        skill.atkInfo.buffer = AssetsManager.Instance.GetActor(ActorType.AACTORBUFFER) as AAcotrBuffer;
        skill.atkInfo.buffer.SetBufferMsg(target, new BufferMsg(BufferType.BT_ADD_HP_CONTINUE, -10, 0));
        skill.atkInfo.buffer.transform.SetParent(skill.transform);
        skill.atkInfo.buffer.gameObject.SetActive(false);
    }

    private void ReleaseSkillTest()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            distanceCircle.gameObject.SetActive(true);
            distanceCircle.transform.localScale = new Vector3(skillMsg.distance * 2, skillMsg.distance * 2, 1);
            if (skillMsg.drParam3 != 0)
            {
                skillCircle.gameObject.SetActive(true);
                skillCircle.transform.localScale = new Vector3(skillMsg.drParam1 * 2, skillMsg.drParam1 * 2, 1);
            }
            else
            {
                skillSquare.gameObject.SetActive(true);
                skillSquare.transform.localScale = new Vector3(skillMsg.drParam1 * 2, skillMsg.drParam2 * 2, 1);
            }
        }
        if (Vector3.Distance(floorMousePos, myAActor.transform.position) <= skillMsg.distance)
        {
            skillCircle.transform.position = floorMousePos;
            skillSquare.transform.position = floorMousePos;
        }
        else
        {
            Vector3 pos = (floorMousePos - myAActor.transform.position).normalized * skillMsg.distance;
            skillCircle.transform.position = pos + myAActor.transform.position;
            skillSquare.transform.position = pos + myAActor.transform.position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            distanceCircle.gameObject.SetActive(false);
            skillCircle.gameObject.SetActive(false);
            skillSquare.gameObject.SetActive(false);
        }
    }

    private void CameraCtrl()
    {
        Vector3 mousePos = Input.mousePosition;
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        if ((mousePos.x <= 0 && horizontal < 0) || (mousePos.x >= Screen.width && horizontal > 0))
            mainCamera.transform.position += Vector3.right * horizontal * Time.deltaTime * cameraMoveSpeed;
        if ((mousePos.y <= 0 && vertical < 0) || (mousePos.y >= Screen.height && vertical > 0))
            mainCamera.transform.position += Vector3.forward * vertical * Time.deltaTime * cameraMoveSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
            mainCamera.transform.position = new Vector3(myAActor.transform.position.x, mainCamera.transform.position.y, myAActor.transform.position.z - 3);
    }
}
