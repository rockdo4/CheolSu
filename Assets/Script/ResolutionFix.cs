using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ResolutionFix : MonoBehaviour
{
    private const float screenWidth = 1920f;
    private Camera thisCamera;
    public Player player;
    private Vector3 originsPos;
    private float offset;

    private void Awake()
    {
        thisCamera = GetComponent<Camera>();
        Rect rect = thisCamera.rect;

        //카메라 비율에 맞추기
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        thisCamera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);

    private void Start()
    {
        //offset = (thisCamera.transform.position.x - player.transform.position.x) / screenWidth;
    }
    private void Update()
    {
        //var temp = offset * Screen.width;
        
        //thisCamera.transform.position = new Vector3(player.transform.position.x + temp, thisCamera.transform.position.y, thisCamera.transform.position.z);
    }
}
