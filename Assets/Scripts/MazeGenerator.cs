using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    private static Dictionary<int, GridCell> _cells = new Dictionary<int, GridCell>();
    private static List<int> _currentSeeds = new List<int>();
    private static Vector3 _lastCameraPosition;

    public static void Build(Camera camera, GameObject container)
    {
        if (camera.transform.position == _lastCameraPosition)
            return;

        var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0f, 0f, camera.transform.position.y));
		var topRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, camera.transform.position.y));

		int x1 = (int)Mathf.Round(bottomLeft.x);
		int x2 = (int)Mathf.Round(topRight.x);
		int y1 = (int)Mathf.Round(bottomLeft.z);
		int y2 = (int)Mathf.Round(topRight.z);

        _currentSeeds.Clear();

		for (int x = x1; x <= x2; x++)
		{
			for (int y = y1; y <= y2; y++)
			{
                int seed = NumberUtil.FromVectorToN(x, y);
                _currentSeeds.Add(seed);
                if (_cells.ContainsKey(seed))
                    continue;
    			
                GridCell cell = GridCell.GetCell(x, y, seed);
				cell.Build(container);
                _cells[seed] = cell;
			}
		}

        var oldSeeds = new List<int>(_cells.Keys);
        foreach (int oldSeed in oldSeeds)
        {
            if (!_currentSeeds.Contains(oldSeed))
            {
                _cells[oldSeed].Disable();
                _cells.Remove(oldSeed);
            }
        }

        _lastCameraPosition = camera.transform.position;
    }

}