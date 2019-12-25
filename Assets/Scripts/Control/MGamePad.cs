using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGamePad : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR
        Vector2 leftAxis = new Vector2(Input.GetAxis("Left X Axis"), Input.GetAxis("Left Y Axis"));
        Vector2 rightAxis = new Vector2(Input.GetAxis("Right X Axis"), Input.GetAxis("Right Y Axis"));

        if(leftAxis.sqrMagnitude > 0.25f)
            InputProperities.Instance.SetRotationL(leftAxis);
        if(rightAxis.sqrMagnitude > 0.25f)
            InputProperities.Instance.SetRotationR(rightAxis);
#endif
    }
}
