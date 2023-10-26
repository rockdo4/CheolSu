using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Update()
    {
        var message = string.Empty;

        Debug.Log(Input.touchCount);
        foreach(var touch in Input.touches)
        {
            message += "Touch ID: " + touch.fingerId;
            message += "\nPhase: " + touch.phase;
            message += "\nPosition: " + touch.position;
            message += "\nDeltaPos: " + touch.deltaPosition;
            message += "\nDeltaTime: " + touch.deltaTime + "\n";
        }
        
        message += "\n";
        text.text = message;
    }
}
