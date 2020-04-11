using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLegTrigger : MonoBehaviour
{
    public bool IsTrigger
    {
        get
        {
            return triggerCount > 0;
        }
    }

    int triggerCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.GetMask("Player"))
        {
            ++triggerCount;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.GetMask("Player"))
        {
            --triggerCount;
        }

    }
}
