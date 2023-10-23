using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public TextMeshProUGUI text;
    private float start;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > start + 0.5f)
        {
            text.enabled = !text.enabled;
            start = Time.time;
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }
}
