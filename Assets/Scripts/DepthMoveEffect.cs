using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthMoveEffect : MonoBehaviour {
    const int MAX_LEVEL = 10;

    [Range(0, MAX_LEVEL - 1)]
    public int level = 1;

    private Vector2 StartPosition;
    private Vector2 CameraStartPosition;
	void Start () {
        CameraStartPosition = Camera.main.transform.position;
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        Vector2 CaemraMoveDistance = (Vector2)Camera.main.transform.position - CameraStartPosition;
        transform.position = StartPosition + CaemraMoveDistance / (MAX_LEVEL - level);
	}
}
