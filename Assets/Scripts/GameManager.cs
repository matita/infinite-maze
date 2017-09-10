using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public GameObject MazeContainer;
	public Camera MainCamera;


	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(Instance);

		Instance = this;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		MazeGenerator.Build(MainCamera, MazeContainer);		
	}
}
