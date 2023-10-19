using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public Transform stage_a;
    public Transform stage_b;
}
public class StageChanger : MonoBehaviour
{
    [SerializeField]
    public Stage[] stages = null;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetStage(int stage)
    {
        if(stage >= stages.Length || stage < 0) { return; }
        for(int i=0; i<stages.Length; i++)
        {
            stages[i].stage_a.gameObject.SetActive(false);
            stages[i].stage_b.gameObject.SetActive(false);

            if (i == stage)
            {
                stages[i].stage_a.gameObject.SetActive(true);
                stages[i].stage_b.gameObject.SetActive(true);
            }
        }
    }
}
