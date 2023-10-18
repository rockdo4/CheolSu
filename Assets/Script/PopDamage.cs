using System.Collections;
using TMPro;
using UnityEngine;

public class PopDamage : PoolAble
{
    public TextMeshProUGUI damage;
    private float start;

    private void OnEnable()
    {
        damage.SetText("10");
        start = Time.time;

        StartCoroutine(ReleaseDamage());
    }

    void Update()
    {
        damage.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), (Time.time - start)/4);
        gameObject.transform.position += Vector3.up * Time.deltaTime;
    }

    IEnumerator ReleaseDamage()
    {
        yield return new WaitForSeconds(3f);
        ReleaseObject();
    }
}
