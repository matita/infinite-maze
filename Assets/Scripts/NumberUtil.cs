using UnityEngine;

public class NumberUtil {

	public static int FromZToN(int z)
	{
		// positive numbers turned to positive even number
		// negative number truned to positive odd number
		return z >= 0 ? 2 * z : -2 * z - 1;
	}

	public static int FromVectorToN(int x, int y)
	{
		x = FromZToN(x);
		y = FromZToN(y);
		var k = x + y;
		return  k * (k+1) / 2 + x;
	}


	public static bool PolygonContainsPoint(Vector2[] vertices, Vector2 point)
	{
   		/*
		var j = vertices.Length - 1; 
   		var inside = false; 
		for (int i = 0; i < vertices.Length; j = i++) { 
			if ( ((vertices[i].y <= point.y && point.y < vertices[j].y) || (vertices[j].y <= point.y && point.y < vertices[i].y)) && 
				(point.x < (vertices[j].x - vertices[i].x) * (point.y - vertices[i].y) / (vertices[j].y - vertices[i].y) + vertices[i].x)) 
				inside = !inside; 
		} 
		return inside; 
		/*/

		float x = point.x;
		float y = point.y;
		
		var inside = false;
		for (int i = 0, j = vertices.Length - 1; i < vertices.Length; j = i++) {
			float xi = vertices[i].x;
			float yi = vertices[i].y;
			float xj = vertices[j].x;
			float yj = vertices[j].y;
			
			var intersect = ((yi > y) != (yj > y))
				&& (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
			if (intersect) inside = !inside;
		}
		
		return inside;

		//*/
	}

	public static Vector2 V3ToV2(Vector3 v)
	{
		return new Vector2(v.x, v.z);
	}

	public static Vector3 V2ToV3(Vector2 v, float y = 0f)
	{
		return new Vector3(v.x, y, v.y);
	}
}
