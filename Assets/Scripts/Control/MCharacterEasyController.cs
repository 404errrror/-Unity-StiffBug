using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MCharacterEasyController : MonoBehaviour
{
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(InputProperities.Instance.isControl_L && rigid.velocity.sqrMagnitude < 10.0f)
        {
            if(InputProperities.Instance.rotation_L >= 0 && InputProperities.Instance.rotation_L < 180.0f)
            {
                rigid.AddForce(Vector2.right * 10.0f);
            }
            else
            {
                rigid.AddForce(Vector2.left * 10.0f);
            }
        }
    }
}
