/*
 * 2019.12.07
 * 게임패드, 조이스틱으로 입력데이터를 받고, InputProperties를 업데이트합니다.
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class MInputController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        JoyStick_Start();
    }

    // Update is called once per frame
    void Update()
    {
        GamePad_Update();
        JoyStick_Update();
    }

    #region GamePad
    /* 게임 패드 조이스틱의 민감도입니다. 범위 : 0.0f ~ 1.0f */
    /* 민감도가 너무 높으면 조이스틱을 뗄 때에도 반응하게 되어 정확도가 낮아질 수 있습니다. */
    public float GamePad_Sensitive = 0.75f;
    float _GamePad_Sensitive;

    void GamePad_Start()
    {
        const float Root2 = 1.4142135623f;
        GamePad_Sensitive = Mathf.Clamp(GamePad_Sensitive, 0.0f, 1.0f);
        _GamePad_Sensitive = Root2 - (GamePad_Sensitive * Root2);
    }

    void GamePad_Update()
    {
        Vector2 leftAxis = new Vector2(Input.GetAxis("Left X Axis"), Input.GetAxis("Left Y Axis"));
        Vector2 rightAxis = new Vector2(Input.GetAxis("Right X Axis"), Input.GetAxis("Right Y Axis"));

        if (leftAxis.sqrMagnitude > _GamePad_Sensitive)
            InputProperities.Instance.SetRotationL(leftAxis);
        if (rightAxis.sqrMagnitude > _GamePad_Sensitive)
            InputProperities.Instance.SetRotationR(rightAxis);
    }
    #endregion // GamePad


    #region JoyStick
    public MJoyStick JoystickObj_L;
    public MJoyStick JoystickObj_R;

    void JoyStick_Start()
    {
        if (JoystickObj_L == null) Debug.LogError("MInputController.JoyStick_Start(), JoystickObj_L is null");
        if (JoystickObj_R == null) Debug.LogError("MInputController.JoyStick_Start(), JoystickObj_R is null");
    }

    void JoyStick_Update()
    {
        if(JoystickObj_L) InputProperities.Instance.SetRotationL(JoystickObj_L.GetStickPosition());
        if(JoystickObj_R) InputProperities.Instance.SetRotationR(JoystickObj_R.GetStickPosition());
    }
    #endregion // TouchPad JoyStick
}
