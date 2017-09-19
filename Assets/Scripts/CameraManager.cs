using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public Transform target;
	public Vector3 distance = new Vector3(0f, 4f, -5f);
	public float rotation = 0;

	// Use this for initialization
	// Update is called once per frame
	void Update () {
		/*rotation = target.rotation.y - 180f;
		float groundDist = Mathf.Sqrt(distance.x * distance.x + distance.z * distance.z);
		Vector3 rotatedDistance = new Vector3(
			groundDist * Mathf.Cos(Mathf.Deg2Rad * rotation),
			distance.y,
			groundDist * Mathf.Sin(Mathf.Deg2Rad * rotation)
		);
		transform.position = target.position + rotatedDistance;
		transform.rotation = Quaternion.Euler(55f, -90 - rotation, 0f);*/
	}
}
