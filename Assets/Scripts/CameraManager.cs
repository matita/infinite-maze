using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public Transform target;
	public Vector3 distance = new Vector3(0f, 4f, -5f);

	// Use this for initialization
	// Update is called once per frame
	void Update () {
		transform.position = target.position + distance;
	}
}
