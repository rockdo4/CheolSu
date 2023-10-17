using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopDamage : PoolAble
{
    public TextMeshProUGUI damage;


    // Update is called once per frame
    void Update()
    {
        damage.SetText("10");

        gameObject.transform.position += Vector3.up;
        StartCoroutine(ReleaseDamage());
    }

    IEnumerator ReleaseDamage()
    {
        yield return new WaitForSeconds(0.5f);
        ReleaseObject();
    }
}
