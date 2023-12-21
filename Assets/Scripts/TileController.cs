using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum Direction
{
    UpLeft, DownLeft, UpRight, DownRight
}
public class TileController : MonoBehaviour
{

    public Vector2Int coordinate;
    public Transform snapPoint;
    public GameObject greenObject;

    public TileController GetNeighbour(Direction direction)
    {
        var targetCoordinate = coordinate;

        switch (direction)
        {
            case Direction.UpLeft:
                targetCoordinate.x--;
                targetCoordinate.y++;
                break;
            case Direction.DownLeft:
                targetCoordinate.x--;
                targetCoordinate.y--;
                break;
            case Direction.UpRight:
                targetCoordinate.x++;
                targetCoordinate.y++;
                break;
            case Direction.DownRight:
                targetCoordinate.x++;
                targetCoordinate.y--;
                break;
        }
        return TowerManager.Instance.GetTile(targetCoordinate);
    }
    public List<TileController> GetAllNeighbours()
    {
        var result = new List<TileController>();

        var lenght = Enum.GetNames(typeof(Direction)).Length;
        for (int i = 0; i < lenght; i++)
        {
            var tile = GetNeighbour((Direction)i);
            if (tile) result.Add(tile);
        }

        return result;
    }

    public TileController GetRandomNeighbour()
    {
        var tiles = GetAllNeighbours();
        Debug.Log(tiles.Count);
        return tiles[Random.Range(0, tiles.Count)];
    }
}
