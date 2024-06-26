using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private Tilemap worldMap;
    [SerializeField]
    private Tilemap terrainMap;
    [SerializeField]
    private Tilemap waterMap;
    [SerializeField]
    private Tilemap borderMap;

    [SerializeField]
    private Tile waterTile;

    //chance of block being water
    [SerializeField]
    private float waterThreshold; 

    [SerializeField]
    private Tile rockTile;

    //chance of rock given that the tile is not water
    [SerializeField]
    private float rockThreshold;

    //the lower the number the less zoomed, therefore less gradual change
    [SerializeField]
    private float scale;

    private float offsetx, offsety;


    private BoundsInt bounds;
    int minx, miny, maxx, maxy;

    
    // Start is called before the first frame update
    void Start()
    {
        bounds = worldMap.cellBounds;
        minx = bounds.xMin; miny = bounds.yMin;
        maxx = bounds.xMax; maxy = bounds.yMax;
        offsetx = Random.Range(0,10000);
        offsety = Random.Range(0, 10000);
        BorderGeneration();
        float perlin;

        for(int i = minx; i < maxx; i++)
        {
            for(int j = miny; j < maxy; j++)
            {
                perlin = Mathf.PerlinNoise((i + offsetx) / scale,(j + offsety) /scale);
                Debug.Log (perlin); 
                if (perlin <= waterThreshold)
                {
                    waterMap.SetTile(new Vector3Int(i, j), waterTile);
                }else if (Random.Range(0f,1f) < rockThreshold)
                {
                    terrainMap.SetTile(new Vector3Int(i, j), rockTile);
                }
            }
        }
    }

    public void BorderGeneration()
    {
        //map border
        for (int i = minx - 1; i < maxx; i++)
        {
            borderMap.SetTile(new Vector3Int(i, miny - 1), rockTile);
            borderMap.SetTile(new Vector3Int(i, maxy), rockTile);
        }
        for (int i = miny - 1; i < maxy; i++)
        {
            borderMap.SetTile(new Vector3Int(minx-1, i), rockTile);
            borderMap.SetTile(new Vector3Int(maxx, i), rockTile);
        }
        //water/terrain
        //spawn food
        //spawn creatures
    }
}
