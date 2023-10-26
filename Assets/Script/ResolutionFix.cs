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
        
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);

    private void Start()
    {
        //offset = (thisCamera.transform.position.x - player.transform.position.x) / screenWidth;
    }
    private void Update()
    {
		SetCamera();
		//var temp = offset * Screen.width;

		//thisCamera.transform.position = new Vector3(player.transform.position.x + temp, thisCamera.transform.position.y, thisCamera.transform.position.z);
	}

    public void SetCamera()
    {
		Rect rect = thisCamera.rect;

		//카메라 비율에 맞추기
		float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로 비율 설정)
		float scalewidth = ((float)Screen.width / Screen.height) / ((float)21 / 9);

		if (scaleheight < 1) // 정해진 비율보다 세로가 길어지면
		{
			rect.height = scaleheight;
			rect.y = (1f - scaleheight) / 2f;
		}
		else //정해진 비율보다 가로가 길어지면
		{
			rect.width = Screen.width;
			rect.x = (1f - Screen.width) / 2f;
		}
		thisCamera.rect = rect;
	}
}
