using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 100f;
	private Vector3 _velocity;
	private Vector3 _collisionVelocity;
	private float _zSpeed;
	private Rigidbody _rb;
	
	void Start()
	{
		_rb = GetComponent<Rigidbody>();
	}

	void OnCollisionStay(Collision collision)
	{
		var normal = collision.contacts[0].normal;
		if (normal.y != 0f)
			return;
		
		_velocity += normal;
		Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green);
	}

	void OnCollisionExit(Collision collision)
	{
		_collisionVelocity = Vector3.zero;
	}
	
	void FixedUpdate()
	{
		_rb.velocity = (_velocity + _collisionVelocity) * speed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		_velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		//transform.Translate(_xSpeed * speed * Time.deltaTime, 0f, _zSpeed * speed * Time.deltaTime);
	}
}
