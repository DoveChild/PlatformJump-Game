using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrouchEvent : MonoBehaviour,
	IPointerUpHandler, IPointerDownHandler
{
	[SerializeField]
	private bool isDown = false;
	public void OnPointerDown(PointerEventData eventData)
	{
		isDown = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isDown = false;
	}

	public bool Getvalue() { return isDown; }
}
