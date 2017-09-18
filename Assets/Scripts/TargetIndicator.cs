using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicator : MonoBehaviour {

	public Transform target;
	public Transform player;
	public Text distText;
	public float padding = 100f;

	
	void Start()
	{
		
	}
	
	void Update () {
		var camera = Camera.main;
		var targetGround = new Vector3(target.position.x, target.position.z, 0f);
		var playerGround = new Vector3(player.position.x, player.position.z, 0f);
		var dist = targetGround - playerGround;
		
		transform.rotation = Quaternion.FromToRotation(Vector3.right, targetGround - playerGround);
		distText.text = dist.magnitude.ToString("0.0") + "m";

	}
}
