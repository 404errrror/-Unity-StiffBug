using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraLimit : MonoBehaviour {
    public float tweenSpeed = 3;
    [Serializable]
    public class MyRect
    {
        public float top;
        public float bottom;
        public float left;
        public float right;

        public float height { get { return top + bottom; } }
        public float width { get { return left + right; } }
    }
    public MyRect limitRect;
    [HideInInspector] public bool isInit;
    
    GameObject player;
    Camera myCam;
    float camWidth;
    float camHeight;

    public enum RectEnum { Left, Top, Right, Bottom, None };
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        myCam = gameObject.GetComponent<Camera>();
        camHeight = myCam.orthographicSize * 2;
        camWidth = myCam.aspect * camHeight;

        if (player == null)
            Debug.LogError("플레이어를 찾을 수 없습니다. 플레이어 오브젝트의 Tag에 Player를 설정해주세요.");
	}
	
	// Update is called once per frame
	void Update () {
        CameraMove();
        CameraLimit_();

	}

    /// <summary>
    /// 카메라가 플레이어를 따라다니도록 합니다.
    /// </summary>
    void CameraMove()
    {
        transform.position = new Vector3(
            transform.position.x + (player.transform.position.x - transform.position.x) * Time.deltaTime * tweenSpeed,
            transform.position.y + (player.transform.position.y - transform.position.y) * Time.deltaTime * tweenSpeed,
            -10);
    }

    /// <summary>
    /// 카메라 범위를 빨간색 박스 안으로 제한합니다.
    /// </summary>
    void CameraLimit_()
    {
        float camLeft = transform.position.x - camWidth * 0.5f;
        float camRight = transform.position.x + camWidth * 0.5f;
        float camTop = transform.position.y + camHeight * 0.5f;
        float camBottom = transform.position.y - camHeight * 0.5f;

        if (camLeft < limitRect.left)
            transform.position = new Vector3(limitRect.left + camWidth * 0.5f, myCam.transform.position.y, transform.position.z);
        if (camRight > limitRect.right)
            transform.position = new Vector3(limitRect.right - camWidth * 0.5f, myCam.transform.position.y, transform.position.z);
        if (camTop > limitRect.top)
            transform.position = new Vector3(transform.position.x, limitRect.top - camHeight * 0.5f, transform.position.z);
        if (camBottom < limitRect.bottom)
            transform.position = new Vector3(transform.position.x, limitRect.bottom + camHeight * 0.5f, transform.position.z);
    }

    public void Initialize()
    {
        if (isInit == true)
            return;
        
        isInit = true;
        limitRect.left = transform.position.x - 5;
        limitRect.right = transform.position.x + 5;
        limitRect.top = transform.position.y + 5;
        limitRect.bottom = transform.position.y - 5;
    }

    /// <summary>
    /// 찍힌 점중 가장 가까운 점의 위치와 거리를 반환합니다.
    /// </summary>
    public Vector2 FindMinPosition(Vector2 mousePosition, out RectEnum minInfo)
    {
        Vector2 left = new Vector2(limitRect.left, limitRect.height * 0.5f);
        Vector2 right = new Vector2(limitRect.right, limitRect.height * 0.5f);
        Vector2 top = new Vector2(limitRect.width * 0.5f, limitRect.top);
        Vector2 bottom = new Vector2(limitRect.width * 0.5f, limitRect.bottom);

        float leftDis = Vector2.Distance(mousePosition, left);
        float rightDis = Vector2.Distance(mousePosition, right);
        float topDis = Vector2.Distance(mousePosition, top);
        float bottomDis = Vector2.Distance(mousePosition, bottom);

        float distance = Mathf.Min(new float[] { leftDis, rightDis, topDis, bottomDis });

        if (distance == leftDis)
        {
            minInfo = RectEnum.Left;
            return left;
        }
        else if (distance == rightDis)
        {
            minInfo = RectEnum.Right;
            return right;
        }
        else if (distance == topDis)
        {
            minInfo = RectEnum.Top;
            return top;
        }
        else if (distance == bottomDis)
        {
            minInfo = RectEnum.Bottom;
            return bottom;
        }
        else
        {
            minInfo = RectEnum.None;
            return Vector2.zero;
        }
    }

    // 사각형 기즈모를 그립니다.
    void OnDrawGizmosSelected ()
    {
        Vector2 topLeft = new Vector2(limitRect.left, limitRect.top);
        Vector2 topRight = new Vector2(limitRect.right, limitRect.top);
        Vector2 bottomLeft = new Vector2(limitRect.left, limitRect.bottom);
        Vector2 bottomRight = new Vector2(limitRect.right, limitRect.bottom);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

}
