using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIActor : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        Debug.Log(gameObject.name + "->enter");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "->exit");
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "->down");
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "->up");
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "->click");
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "->beginDrag");
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "->drag");
    }
    
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "->endDrag");
    }
}
