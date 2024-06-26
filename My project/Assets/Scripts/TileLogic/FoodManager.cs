using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    //dev options
    [SerializeField]
    private int StartingFoodAmount;
    [SerializeField]
    private float foodSpawnRate = 5f;

    //objects in scene
    [SerializeField]
    private Tilemap foodMap;
    [SerializeField]
    private Tilemap worldMap;
    [SerializeField]
    Transform foodHolder;

    //prefabs
    [SerializeField] 
    private GameObject food;

    //class variables
    private float foodSpawnTimer;
    private BoundsInt mapBorder;
    private Grid grid;

    //Singleton
    public static FoodManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        mapBorder = worldMap.cellBounds;
        foodSpawnTimer = foodSpawnRate;
        for(int i = 0; i < StartingFoodAmount; i++)
        {
            RandomAddFood();    
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(foodSpawnTimer < 0)
        {
            RandomAddFood();
            foodSpawnTimer = foodSpawnRate;
        }
        else
        {
            foodSpawnTimer -= Time.deltaTime;
        }
    }

    private void RandomAddFood()
    {
        int max_tries = 2;
        
        Vector3Int random_position;

        Collider2D collider;

        do
        {
            random_position = new(Random.Range(mapBorder.xMin, mapBorder.xMax), Random.Range(mapBorder.yMin, mapBorder.yMax));
            collider = Physics2D.OverlapCircle(GameManager.Instance.getGrid().CellToWorld(random_position), 0.08f);
            max_tries--;
        } while (collider != null);

        GameObject new_food = Instantiate(food);
        new_food.transform.position = GameManager.Instance.getGrid().GetCellCenterWorld(random_position);
        new_food.transform.parent = foodHolder;
        new_food.name = random_position.ToString();
    }
}
