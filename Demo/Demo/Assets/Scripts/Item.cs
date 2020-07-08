using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : UIActor {
    private Text m_desText;
    private Text m_countsText;
    private Image m_icon;
    private GameObject m_curGrid;

    private ItmeMsg m_itmeMsg;
    // Use this for initialization
    void Start () {
        m_icon = GetComponent<Image>();
        m_curGrid = GameObject.Find("ItemGrid1");
        transform.position = m_curGrid.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        m_icon.raycastTarget = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position = eventData.position;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (eventData.pointerEnter)
        {
            transform.position = eventData.pointerEnter.transform.position;
            m_curGrid = eventData.pointerEnter;
        }
        else
        {
            transform.position = m_curGrid.transform.position;
        }
        m_icon.raycastTarget = true;
    }
}
