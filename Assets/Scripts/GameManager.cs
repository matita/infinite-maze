using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public Material GroundMaterial;
	public Material WallMaterial;
	public GameObject MazeContainer;
	public Camera MainCamera;
	public float MaxCameraDistance = 10f;
	public int MinRoomArea = 0;
	public int MaxRoomArea = 20;
	[Range(3, 30)]
	public int CellsPerSide = 10;
	public float UnitsPerCell = 10f;
	

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
		MainCamera.transform.position += CameraSpeed * Time.deltaTime;
		
		MazeGenerator.Build(MainCamera, MazeContainer, MaxCameraDistance);
		
		if (CubesPool.Count != _lastCubesCount)
		{
			_lastCubesCount = CubesPool.Count;
			Debug.Log("Cubes count: " + _lastCubesCount);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		for (int i = 0; i < MazeGenerator.cameraPolygon.Length; i++)
		{
			var p1 = NumberUtil.V2ToV3(MazeGenerator.cameraPolygon[i]);
			var p2 = NumberUtil.V2ToV3(MazeGenerator.cameraPolygon[(i + 1) % MazeGenerator.cameraPolygon.Length]);
			Gizmos.DrawLine(p1, p2);
		}


		int x1 = (int)Mathf.Round(Mathf.Min(MazeGenerator.cameraPolygon.Select(v=>v.x).ToArray()) / UnitsPerCell);
        int x2 = (int)Mathf.Round(Mathf.Max(MazeGenerator.cameraPolygon.Select(v=>v.x).ToArray()) / UnitsPerCell);
        int y1 = (int)Mathf.Round(Mathf.Min(MazeGenerator.cameraPolygon.Select(v=>v.y).ToArray()) / UnitsPerCell);
        int y2 = (int)Mathf.Round(Mathf.Max(MazeGenerator.cameraPolygon.Select(v=>v.y).ToArray()) / UnitsPerCell);

        
		Gizmos.color = Color.red;
		for (int x = x1; x <= x2; x++)
		{
			for (int y = y1; y <= y2; y++)
			{
                Gizmos.DrawCube(new Vector3((float)x, 0f, (float)y) * UnitsPerCell, Vector3.one*UnitsPerCell*0.1f);
			}
		}
	}
}
