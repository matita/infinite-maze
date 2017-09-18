using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public Transform player;
	private int targetCount;

	// Use this for initialization
	void Start () {
		float cellUnit = GameManager.Instance.UnitsPerCell / GameManager.Instance.CellsPerSide;
		transform.position = new Vector3(-cellUnit / 2, transform.position.y, -cellUnit / 2);
		relocate();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.transform != player)
			return;
			
		targetCount++;
		Debug.Log("Found " + targetCount + " targets");
		relocate();
	}
	
	void relocate()
	{
		var hDiff = Random.Range(2, 5) * (targetCount + 1);
		var hSign = Random.Range(0, 2) == 0 ? -1 : 1;

		var vDiff = Random.Range(2, 5) * (targetCount + 1);
		var vSign = Random.Range(0, 2) == 0 ? -1 : 1;

		float cellUnit = GameManager.Instance.UnitsPerCell / GameManager.Instance.CellsPerSide;

		transform.position += new Vector3(cellUnit * hDiff * hSign, 0f, cellUnit * vDiff * vSign);
	}
}
