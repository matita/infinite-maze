using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 100f;
	public float rotationSensitivity = 400f;
	private float _rotation = 0;
	private Vector3 _velocity;
	private Vector3 _collisionVelocity;
	private Rigidbody _rb;

	private Dictionary<Collider, Vector3> _currentCollisions = new Dictionary<Collider, Vector3>();
	
	void Start()
	{
		_rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision collision)
	{
		foreach (var point in collision.contacts)
		{
			var normal = point.normal;
			if (normal == Vector3.up)
				continue;
			
			//Debug.Log("collider on enter: " + collision.collider);
			_currentCollisions[point.otherCollider] = normal;
			_collisionVelocity = normal;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (!_currentCollisions.ContainsKey(collision.collider))
			return;

		//Debug.Log("collider on exit: " + collision.collider);
		var collisionVector = _currentCollisions[collision.collider];
		_collisionVelocity -= collisionVector;
		_currentCollisions.Remove(collision.collider);
	}
	
	void FixedUpdate()
	{
		_rb.velocity = (_velocity + _collisionVelocity) * speed * Time.deltaTime;
		_rb.angularVelocity = new Vector3(0f, Input.GetAxis("Mouse X") * 1000f * Time.deltaTime, 0f);
	}
	
	void Update () {
		_rotation += (Input.GetAxis("Mouse X") + Input.GetAxis("Right X")) * rotationSensitivity * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
		_velocity = transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
	}
}
