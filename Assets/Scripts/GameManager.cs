using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public GameObject MazeContainer;
	public Camera MainCamera;
	public int MinRoomArea = 0;
	public int MaxRoomArea = 20;
	

	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(Instance);

		Instance = this;
	}

	void Start()
	{
		nextPos = MainCamera.transform.position;
	}


	public Vector3 CameraSpeed;
	public float MaxDistance = 5f;
	public float Threshold = 1f;
	public float CameraFriction = 0.95f;

	private int _lastCubesCount = 0;
	private Vector3 nextPos;

	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			var mousePosition = Input.mousePosition;
			mousePosition.z = MainCamera.transform.position.y;
			Debug.Log("Mouse: " + mousePosition);
			var worldPos = MainCamera.ScreenToWorldPoint(mousePosition);
			Debug.Log("Mouse: " + worldPos);
			worldPos.y = MainCamera.transform.position.y;
			nextPos = worldPos;
		}

		Vector3 diff = nextPos - MainCamera.transform.position;
		if (diff.magnitude <= Threshold)
		{
			//nextPos = MainCamera.transform.position + new Vector3(Random.Range(-MaxDistance, MaxDistance), 0.0f, Random.Range(-MaxDistance, MaxDistance));
			//diff = nextPos - MainCamera.transform.position;
			//Debug.Log("Next pos: " + nextPos);
			//Debug.Log("Diff: " + diff);
		}

		//MainCamera.transform.position += diff * CameraFriction * Time.deltaTime;
		
		MazeGenerator.Build(MainCamera, MazeContainer);
		
		if (CubesPool.Count != _lastCubesCount)
		{
			_lastCubesCount = CubesPool.Count;
			Debug.Log("Cubes count: " + _lastCubesCount);
		}
	}
}
