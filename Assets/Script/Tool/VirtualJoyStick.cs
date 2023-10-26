using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public enum Axis
    {
        Horizontal,
        Vertical,
    }
    public Image stick;
    public float radius;
    private Vector3 originalPoint = Vector3.zero;
	private RectTransform rectTr;

	private Vector2 value;
	private int pointerId;
	private bool isDragging = false;	

	private void Start()
	{
		originalPoint = stick.rectTransform.position;
		rectTr = GetComponent<RectTransform>();
	}

	private void Update()
	{
		Debug.Log($"{GetAxis(Axis.Horizontal)} / {GetAxis(Axis.Vertical)}");
	}
	public float GetAxis(Axis axis)
    {
        switch(axis)
		{
			case Axis.Horizontal:
				return value.x;
			case Axis.Vertical:
				return value.y;
		}
		return 0f;
    }
	public void UpdateStickPos(Vector3 screenPos)
	{
		RectTransformUtility.ScreenPointToWorldPointInRectangle(
			rectTr, screenPos, null, out Vector3 newPoint);

		var delta = Vector3.ClampMagnitude(newPoint - originalPoint, radius);
		stick.rectTransform.position = originalPoint + delta;

		value = delta.normalized;
	}
	public void OnDrag(PointerEventData eventData)
	{
		if (pointerId != eventData.pointerId)
			return;

		UpdateStickPos(eventData.position);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (isDragging) 
			return;

		isDragging = true;
		pointerId = eventData.pointerId;
		UpdateStickPos(eventData.position);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		UpdateStickPos(originalPoint);
		//value = Vector2.zero;
	}
}
