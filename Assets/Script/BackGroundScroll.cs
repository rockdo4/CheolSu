using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public List<RectTransform> backGround;
    private int length;
    private float width;
    private Vector3 offset;

    // Start is called before the first frame update
    private void Start()
    {
        length = backGround.Count;
        width = GetComponent<BoxCollider>().size.x;
        offset = new Vector3(width * 2f, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < length; i++)
        {
            backGround[i].Translate(Vector3.left * Time.deltaTime * (length - i));
            if(backGround[i].position.x < transform.position.x - width)
            {
                backGround[i].position += offset;
            }
        }
    }
}
