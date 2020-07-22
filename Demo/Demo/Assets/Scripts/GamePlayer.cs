using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public AActor myAActor;
    public Skill skill;
    private Camera camera;
    private int maskLayer;
	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        maskLayer = LayerMask.GetMask("OtherPlayer", "Master");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 viewPos = camera.ScreenToViewportPoint(Input.mousePosition);
            viewPos += new Vector3(0, 0, 1);
            Vector3 worldPos = camera.ViewportToWorldPoint(viewPos);
            Vector3 dir = (worldPos - camera.transform.position).normalized;
            RaycastHit hitInfo;
            if (Physics.Raycast(camera.transform.position, dir, out hitInfo, Mathf.Infinity, maskLayer))
            {
                AActor target = hitInfo.transform.GetComponent<AActor>();
                if (target == myAActor)
                    return;
                Skill skill = AssetsManager.Instance.GetActor(ActorType.SKILL) as Skill;
                //skill.transform.SetParent(myAActor.transform.parent);
                skill.transform.position = myAActor.transform.position;
                skill.aimAActor = target;
                skill.atkInfo.buffer = AssetsManager.Instance.GetActor(ActorType.AACTORBUFFER) as AAcotrBuffer;
                skill.atkInfo.buffer.SetBufferMsg(target, new BufferMsg(BufferType.BT_ADD_PATK, 20, 10));
                skill.atkInfo.buffer.transform.SetParent(skill.transform);
                skill.atkInfo.buffer.gameObject.SetActive(false);
            }
        }
	} 
}
