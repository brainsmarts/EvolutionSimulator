using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap worldMap;
    [SerializeField]
    private float timeScale = 1;

    [SerializeField]
    private Tilemap terrainMap;

    private BoundsInt mapBorder;
    private int minx, maxx, miny, maxy; 
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        mapBorder = worldMap.cellBounds;
        minx = mapBorder.xMin; maxx = mapBorder.xMax;
        miny = mapBorder.yMin; maxy = mapBorder.yMax;
        Time.timeScale = timeScale;    
    }

    // Update is called once per frame

    public Grid getGrid()
    {
        return grid;
    }

    public bool OutOfBounds(Vector3Int position)
    {
        /*Debug.Log("Out of bounds Logging + /nDesiredPosiiton: "+ position.x + " " + position.y + 
            "\nMinimumBoundry" + minx + " " + miny +
            "\nMaximumBoundry" + maxx + " " + maxy);*/
        return position.x >= maxx || position.x <= minx-1 || position.y >= maxy || position.y <= miny-1;
    }

    public bool IsNotRock(Vector3Int position)
    {
        //Debug.Log("Position " + position + " Is not rock " + terrain_map.GetTile(position) == null);
        return terrainMap.GetTile(position) == null;
    }
}

