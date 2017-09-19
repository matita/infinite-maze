using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 100f;
	private Vector3 _velocity;
	private Vector3 _collisionVelocity;
	private float _zSpeed;
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
			Debug.Log("enter collisionVelocity: " + _collisionVelocity);
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
		Debug.Log("exit collisionVelocity: " + _collisionVelocity);
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
