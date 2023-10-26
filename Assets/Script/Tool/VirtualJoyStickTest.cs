using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualJoyStickTest : MonoBehaviour
{
	public VirtualJoyStick2 joystick;

	private void Update()
	{
		Debug.Log($"{ joystick.Value.x}, {joystick.Value.y}");
	}
}
