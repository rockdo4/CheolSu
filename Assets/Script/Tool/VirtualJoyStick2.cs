using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoyStick2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public Vector2 Value { get; private set; }

	private int pointerId;
	private bool IsDragging = false;

	private void Update()
	{
		Debug.Log(Value);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (pointerId != eventData.pointerId)
			return;

		Value = eventData.position / Screen.dpi;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (IsDragging)
			return;

		IsDragging = true;
		pointerId = eventData.pointerId;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (pointerId != eventData.pointerId)
			return;

		IsDragging = false;
	}
}