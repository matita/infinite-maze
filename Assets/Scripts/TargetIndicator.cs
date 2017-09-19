using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour {

	public Transform target;
	public Transform player;
	public Transform arm;
	public Text distText;
	public float padding = 100f;

	
	void Start()
	{
		
	}
	
	void Update () {
		var camera = Camera.main;

		var targetScreenPos = camera.WorldToScreenPoint(new Vector3(target.position.x, 0f, target.position.z));
		var distFromTargetScreen = targetScreenPos - new Vector3(camera.pixelWidth / 2f, camera.pixelHeight / 2f, targetScreenPos.z);
		
		if (distFromTargetScreen.magnitude < padding)
			arm.gameObject.SetActive(false);
		else
			arm.gameObject.SetActive(true);

		var targetGround = new Vector3(target.position.x, target.position.z, 0f);
		var playerGround = new Vector3(player.position.x, player.position.z, 0f);
		var dist = targetGround - playerGround;
		
		transform.rotation = Quaternion.FromToRotation(Quaternion.Euler(0f, 0f, -player.rotation.eulerAngles.y) * Vector3.right, dist);
		distText.text = dist.magnitude.ToString("0.0") + "m";

	}
}
