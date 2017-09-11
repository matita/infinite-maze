using System.Collections.Generic;
using UnityEngine;

public class CubesPool
{
    public static int Count = 0;
    private static Queue<GameObject> _pool = new Queue<GameObject>();

    public static GameObject Get()
    {
        if (_pool.Count == 0)
        {
            Count++;
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        
        GameObject obj = _pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }


    public static void Release(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}