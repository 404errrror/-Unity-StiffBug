/* 
 * 2019.11.10
 * 캐릭터 컨트롤러입니다.
 * InputManager 에서 데이터를 가져와 처리합니다.
 */
using System;
using UnityEngine;

public class MCharacterController : MonoBehaviour
{
    enum LandState
    {
        LandState_None = 0,
        LandState_Left,
        LandState_Right,
        LandState_All,
    }

    public GameObject face;
    public GameObject leftLeg;
    public GameObject rightLeg;
    public MLegTrigger leftTrigger;
    public MLegTrigger rightTrigger;

    float lastRotate;
    LandState landState;
    Rigidbody2D rigidbody;

    Vector2 leftLegOriginLocPos;
    Vector2 rightLegOriginLocPos;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        leftLegOriginLocPos = leftLeg.transform.localPosition;
        rightLegOriginLocPos = rightLeg.transform.localPosition;
    }

    void Update()
    {
        LandState newLandState = TriggerCheck();

        if (newLandState != landState)
        {
            UpdateLandState(newLandState);
        }

        UpdateLegRotation();
        FaceRotateLock();
        LegRoateLock();
    }

    void UpdateLegRotation()
    {
        /* Null Check */
        if (leftLeg == null)
        {
            Debug.LogWarning("CharacterController.UpdateLegRotation(), leftLeg is null!!");
            return;
        }
        if (rightLeg == null)
        {
            Debug.LogWarning("CharacterController.UpdateLegRotation(), rightLeg is null!!");
            return;
        }
        if(leftTrigger == null)
        {
            Debug.LogWarning("CharacterController.UpdateLegRotation(), leftTrigger is null!!");
            return;
        }
        if (rightTrigger == null)
        {
            Debug.LogWarning("CharacterController.UpdateLegRotation(), rightTrigger is null!!");
            return;
        }

        switch (landState)
        {
            case LandState.LandState_All:
            case LandState.LandState_None:
                {
                    rightLeg.transform.eulerAngles = new Vector3(0.0f, 0.0f, LimitAngle(0.0f, 180.0f, InputProperities.Instance.rotation_R));
                    leftLeg.transform.eulerAngles = new Vector3(0.0f, 0.0f, LimitAngle(-180.0f, 0.0f, InputProperities.Instance.rotation_L));
                }
                break;

            case LandState.LandState_Left:
                {
                    /* Rotate Pivot Using LeftLeg */
                    float rotation = LimitAngle(-180.0f, 0.0f, InputProperities.Instance.rotation_L);
                    transform.parent.eulerAngles = new Vector3(0.0f, 0.0f,
                            rotation - leftLeg.transform.localEulerAngles.z
                            );

                    /* Rotate RightLeg */
                    rotation = LimitAngle(0.0f, 180.0f, InputProperities.Instance.rotation_R);
                    rightLeg.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);

                }
                break;

            case LandState.LandState_Right:
                {
                    /* Rotate Pivot Using RightLeg */
                    float rotation = LimitAngle(0.0f, 180.0f, InputProperities.Instance.rotation_R);
                    transform.parent.eulerAngles = new Vector3(0.0f, 0.0f,
                            rotation - rightLeg.transform.localEulerAngles.z);

                    /* Rotate LeftLeg */
                    rotation = LimitAngle(-180.0f, 0.0f, InputProperities.Instance.rotation_L);
                    leftLeg.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);
                }
                break;

            default:
                break;
        }


    }

    /* 트리거를 체크하여 현재 LandState를 알아내어 반환합니다.  */
    private LandState TriggerCheck()
    {
        if (leftTrigger.IsTrigger && rightTrigger.IsTrigger)
            return LandState.LandState_All;

        else if (leftTrigger.IsTrigger)
            return LandState.LandState_Left;

        else if (rightTrigger.IsTrigger)
            return LandState.LandState_Right;

        else
            return LandState.LandState_None;
    }

    /* 새로운 LandState로 업데이트 합니다. */
    private void UpdateLandState(LandState newLandState)
    {
        switch (newLandState)
        {
            case LandState.LandState_None:
            case LandState.LandState_All:
                ActivateGravity(true);
                break;

            case LandState.LandState_Left:
                SetPositionOnlyParent(leftTrigger.transform.position);
                ActivateGravity(false);
                break;
            case LandState.LandState_Right:
                SetPositionOnlyParent(rightTrigger.transform.position);
                ActivateGravity(false);
                break;

            default:
                break;
        }

        landState = newLandState;
    }

    /* 피봇(부모)의 위치만 바꿉니다. */
    private void SetPositionOnlyParent(Vector3 newPosition)
    {
        // 서순 주의!!
        transform.position += transform.parent.position - newPosition;
        transform.parent.position = newPosition;
    }

    /* 중력을 활성화/비활성화 합니다. */
    private void ActivateGravity(bool isActive)
    {
        if (isActive)
        {
            rigidbody.gravityScale = 1.0f;
        }
        else
        {
            rigidbody.gravityScale = 0.0f;
            rigidbody.velocity = Vector2.zero;
        }
    }

    private float LimitAngle(float min, float max, float value)
    {
        if(value > max || value < min)
        {
            /* min, max에서 value의 각도가 얼마나 떨어져있는지 계산. */
            float distanceFromMin = Mathf.Abs(min - value);
            float distanceFromMax = Mathf.Abs(max - value);

            if (distanceFromMin > 180.0f)
                distanceFromMin = (distanceFromMin - 360.0f) * -1.0f;
            if (distanceFromMax > 180.0f)
                distanceFromMax = (distanceFromMax - 360.0f) * -1.0f;

            if (distanceFromMax < distanceFromMin)
            {
                return max;
            }
            else return min;
        }

        return value;
    }
    
    /* Face Object의 World Rotation을 Zero로 고정시킵니다. */
    private void FaceRotateLock()
    {
        face.transform.rotation = Quaternion.identity;
    }

    /* 다리의 너비(사타구니?)가 회전되지 않게 고정합니다. */
    private void LegRoateLock()
    {
        leftLeg.transform.position = (Vector2)transform.position + leftLegOriginLocPos;
        rightLeg.transform.position = (Vector2)transform.position + rightLegOriginLocPos;
    }
}
