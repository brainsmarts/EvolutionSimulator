using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private Tilemap world_map;
    [SerializeField]
    private Tilemap terrain_map;
    [SerializeField]
    private Tilemap water_map;
    [SerializeField]
    private Tilemap border_map;

    [SerializeField]
    private Tile water_tile;

    //chance of block being water
    [SerializeField]
    private float water_threshhold; 

    [SerializeField]
    private Tile rock_tile;

    //chance of rock given that the tile is not water
    [SerializeField]
    private float rock_threshhold;

    //the lower the number the less zoomed, therefore less gradual change
    [SerializeField]
    private float scale;

    private float offsetx, offsety;


    private BoundsInt bounds;
    int minx, miny, maxx, maxy;

    
    // Start is called before the first frame update
    void Start()
    {
        bounds = world_map.cellBounds;
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
                if (perlin <= water_threshhold)
                {
                    water_map.SetTile(new Vector3Int(i, j), water_tile);
                }else if (Random.Range(0f,1f) < rock_threshhold)
                {
                    terrain_map.SetTile(new Vector3Int(i, j), rock_tile);
                }
            }
        }
    }

    public void BorderGeneration()
    {
        //map border
        for (int i = minx - 1; i < maxx; i++)
        {
            border_map.SetTile(new Vector3Int(i, miny - 1), rock_tile);
            border_map.SetTile(new Vector3Int(i, maxy), rock_tile);
        }
        for (int i = miny - 1; i < maxy; i++)
        {
            border_map.SetTile(new Vector3Int(minx-1, i), rock_tile);
            border_map.SetTile(new Vector3Int(maxx, i), rock_tile);
        }
        //water/terrain
        //spawn food
        //spawn creatures
    }
}
