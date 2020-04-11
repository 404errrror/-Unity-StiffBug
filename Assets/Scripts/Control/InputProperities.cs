/*
 *  2019.11.10
 *  입력과 관련된 데이터를 가지고 관리합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProperities : Singleton<InputProperities>
{
    public float rotation_L { get; private set; }
    public float rotation_R { get; private set; }

    public bool isControl_L { get; private set; }
    public bool isControl_R { get; private set; }

    public void SetRotationL(Vector2 direction)
    {
        rotation_L = VectorToRotation(direction);
    }

    public void SetRotationR(Vector2 direction)
    {
        rotation_R = VectorToRotation(direction);
    }

    public void SetIsControlL(bool isControl)
    {
        isControl_L = isControl;
    }

    public void SetIsControlR(bool isControl)
    {
        isControl_R = isControl;
    }

    private float VectorToRotation(Vector2 vector)
    {
        // 0 ~ 360 degree
        //if(vector.x < 0 && vector.y < 0)
        //    return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg + 450;
        //else
        //    return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg + 90;

        // -180 ~ 180 degree
        if (vector.x <= 0.0f && vector.y >= 0.0f)
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 270;
        else
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg + 90;
    }
}
