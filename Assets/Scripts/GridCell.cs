using System;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private static List<GridCell> _pool = new List<GridCell>();

    public static GridCell GetCell(int x, int y, int seed)
    {
        GridCell cell;
        if (_pool.Count != 0)
        {
            cell = _pool[0];
            _pool.RemoveAt(0);
        }
        else
        {
            cell = new GridCell();
        }

        cell.reset(x, y, seed);
        return cell;
    }

    public static void Release(GridCell cell)
    {
        _pool.Add(cell);
    }

    
    public int x;
    public int y;
    private int seed;
    private System.Random rnd;

    private float wallWidth = 0.01f;
    private int cellCount = 10;
    private float unit;
    private float startX;
    private float startY;

    private List<GameObject> _cubes = new List<GameObject>();


    private GridCell() { }

    public void Disable()
    {
        foreach (var cube in _cubes)
            CubesPool.Release(cube);
        _cubes.Clear();
        Release(this);
    }

    private void reset(int x, int y, int seed)
    {
        this.cellCount = GameManager.Instance.CellsPerSide;
        this.x = x;
        this.y = y;
        this.seed = seed;
        this.rnd = new System.Random(seed);
        unit = 1f / cellCount;
        this.startX = this.x - 0.5f;
        this.startY = this.y - 0.5f;
    }


    internal void Build(GameObject container)
    {
        buildLine(cellCount, 0, cellCount, container, WallDirection.Vertical);
        buildLine(0, cellCount, cellCount, container, WallDirection.Horizontal);
    
        splitRoom(0, 0, cellCount, cellCount, container);

        buildFloor(container);
    }

    private void buildFloor(GameObject container)
    {
        var floor = CubesPool.Get();
        floor.transform.position = new Vector3((float)this.x, -unit, (float)this.y) * GameManager.Instance.UnitsPerCell;
        floor.transform.localScale = new Vector3(1f, unit, 1f) * GameManager.Instance.UnitsPerCell;
        floor.GetComponent<BoxCollider>().material = GameManager.Instance.SlipperyMaterial;
        floor.GetComponent<MeshRenderer>().material = GameManager.Instance.GroundMaterial;

        floor.transform.SetParent(container.transform);
        _cubes.Add(floor);
    }


    private void buildWall(int x, int y, int size, GameObject container, WallDirection direction)
    {
        float xPos, yPos, width, height;

        if (direction == WallDirection.Vertical)
        {
            xPos = startX + x * unit/* - unit/2f*/;
            yPos = startY + (y + size / 2f) * unit;
            width = wallWidth;
            height = unit * size;
        }
        else
        {
            xPos = startX + (x + size / 2f) * unit;
            yPos = startY + y * unit/* - unit/2f*/;
            width = unit * size;
            height = wallWidth;
        }

        var cube = CubesPool.Get();
        cube.transform.position = new Vector3(xPos, 0f, yPos) * GameManager.Instance.UnitsPerCell;
        cube.transform.localScale = new Vector3(width, unit, height) * GameManager.Instance.UnitsPerCell;
        cube.GetComponent<BoxCollider>().material = GameManager.Instance.SlipperyMaterial;
        cube.GetComponent<MeshRenderer>().material = GameManager.Instance.WallMaterial;
        cube.transform.SetParent(container.transform);

        _cubes.Add(cube);
    }

    private void buildLine(int x, int y, int size, GameObject container, WallDirection direction)
    {
        var gapI = this.rnd.Next(0, size);

        if (gapI > 0)
            buildWall(x, y, gapI, container, direction);
        if (gapI + 1 < size)
        {
            int nextSize = size - (gapI + 1);
            buildWall(x + (direction == WallDirection.Horizontal ? gapI + 1 : 0), y + (direction == WallDirection.Vertical ? gapI + 1 : 0), nextSize, container, direction);
        }
    }


    private void splitRoom(int x, int y, int w, int h, GameObject container, int level = 0)
    {
        if (w <= 1 || h <= 1)
            return;
        
        var maxArea = this.rnd.Next(GameManager.Instance.MinRoomArea, GameManager.Instance.MaxRoomArea);
        if (w * h <= maxArea)
            return;

        var isHorizontal = w == h ? this.rnd.Next(0, 1) == 0 : w > h;
        if (isHorizontal)
            splitV(x, y, w, h, container, level);
        else
            splitH(x, y, w, h, container, level);
    }

    private void splitV(int x, int y, int w, int h, GameObject container, int level)
    {
        if (w <= 1)
            return;

        var min = Mathf.Max(1, Mathf.Round((float)(w)/4f));
        var max = Mathf.Round(((float)w / 4f) * 3f);
        var firstW = this.rnd.Next((int)min, (int)max); //this.rnd.Next(1, w);
        var lineX = x + firstW;
        buildLine(lineX, y, h, container, WallDirection.Vertical);

        splitRoom(x, y, firstW, h, container, level + 1);
        splitRoom(lineX, y, w - firstW, h, container, level + 1);
    }

    private void splitH(int x, int y, int w, int h, GameObject container, int level)
    {
        if (h <= 1)
            return;

        var firstH = this.rnd.Next(1, h); //1 + Math.floor(rnd() * (h-1));
        var lineY = y + firstH;
        //RoomUtils.lineH(rnd, lines, x, lineY, w);
        buildLine(x, lineY, w, container, WallDirection.Horizontal);

        //RoomUtils.splitRoom(rnd, lines, x, y, w, firstH, level + 1);
        //RoomUtils.splitRoom(rnd, lines, x, lineY, w, h - firstH, level + 1);
        splitRoom(x, y, w, firstH, container, level + 1);
        splitRoom(x, lineY, w, h - firstH, container, level + 1);
    }
}

enum WallDirection
{
    Horizontal,
    Vertical
}