using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGenerator
{
    private static Dictionary<int, GridCell> _cells = new Dictionary<int, GridCell>();
    private static List<int> _currentSeeds = new List<int>();
    private static Vector3 _lastCameraPosition;
    private static Quaternion _lastCameraRotation;
    private static Plane ground = new Plane(Vector3.up, Vector3.zero);
    private static bool hasCameraPlanes = false;
    public static Vector2[] cameraPolygon = new Vector2[] {};

    public static void Build(Camera camera, GameObject container, float maxDistance)
    {
        if (camera.transform.position.y <= 0)
            return;

        if (camera.transform.position == _lastCameraPosition && camera.transform.rotation == _lastCameraRotation)
            return;

        var bottomLeft = getGroundIntersection(camera, new Vector3(0f, 0f, 0f), maxDistance);
        var topLeft = getGroundIntersection(camera, new Vector3(0f, camera.pixelHeight, 0f), maxDistance);
        var topRight = getGroundIntersection(camera, new Vector3(camera.pixelWidth, camera.pixelHeight, 0f), maxDistance);
        var bottomRight = getGroundIntersection(camera, new Vector3(camera.pixelWidth, 0f, 0f), maxDistance);
        cameraPolygon = new Vector2[] {
            NumberUtil.V3ToV2(bottomLeft), 
            NumberUtil.V3ToV2(bottomRight), 
            NumberUtil.V3ToV2(topRight), 
            NumberUtil.V3ToV2(topLeft)
        };

        int x1 = (int)Mathf.Round(Mathf.Min(MazeGenerator.cameraPolygon.Select(v=>v.x).ToArray()) / GameManager.Instance.UnitsPerCell);
        int x2 = (int)Mathf.Round(Mathf.Max(MazeGenerator.cameraPolygon.Select(v=>v.x).ToArray()) / GameManager.Instance.UnitsPerCell);
        int y1 = (int)Mathf.Round(Mathf.Min(MazeGenerator.cameraPolygon.Select(v=>v.y).ToArray()) / GameManager.Instance.UnitsPerCell);
        int y2 = (int)Mathf.Round(Mathf.Max(MazeGenerator.cameraPolygon.Select(v=>v.y).ToArray()) / GameManager.Instance.UnitsPerCell);

        //x1 = x2 = y1 = y2 = 0;

        //x2 = Mathf.Min(x2, x1 + 10);
        //y2 = Mathf.Min(y2, y1 + 10);

        _currentSeeds.Clear();

		for (int x = x1; x <= x2; x++)
		{
			for (int y = y1; y <= y2; y++)
			{
                /*if (!NumberUtil.PolygonContainsPoint(cameraPolygon, new Vector2((float)x, (float)y)))
                    continue;*/

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
        _lastCameraRotation = camera.transform.rotation;
    }

    private static Vector3 getGroundIntersection(Camera camera, Vector3 screenPosition, float maxDistance)
    {
        Ray ray = camera.ScreenPointToRay(screenPosition);
        float distance = 0;

        if (ground.Raycast(ray, out distance))
            return ray.GetPoint(Mathf.Min(distance, maxDistance));
        
        return ray.GetPoint(maxDistance);
    }


    private static void buildCameraPlanes(Camera camera)
    {
        
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        int i = 0;
        while (i < planes.Length) {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
            i++;
        }

        hasCameraPlanes = true;
    }

}