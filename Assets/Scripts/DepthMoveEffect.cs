using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthMoveEffect : MonoBehaviour {

    [Range(0,10)]
    public int level = 1;

    Vector2 position;
	void Start () {
        position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = (Vector2)Camera.main.transform.position + (position - (Vector2)Camera.main.transform.position) * (1.0f - (level * 0.1f));
	}
}
