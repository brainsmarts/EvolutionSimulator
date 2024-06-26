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
    private float food_spawn_rate = 5f;

    //objects in scene
    [SerializeField]
    private Tilemap food_map;
    [SerializeField]
    private Tilemap world_map;
    [SerializeField]
    Transform food_holder;

    //prefabs
    [SerializeField] 
    private GameObject food;

    //class variables
    private float food_spawn_timer;
    private BoundsInt map_border;
    private Grid grid;

    //Singleton
    public static FoodManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        map_border = world_map.cellBounds;
        food_spawn_timer = food_spawn_rate;
        for(int i = 0; i < StartingFoodAmount; i++)
        {
            RandomAddFood();    
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(food_spawn_timer < 0)
        {
            RandomAddFood();
            food_spawn_timer = food_spawn_rate;
        }
        else
        {
            food_spawn_timer -= Time.deltaTime;
        }
    }

    private void RandomAddFood()
    {
        int max_tries = 2;
        
        Vector3Int random_position;

        Collider2D collider;

        do
        {
            random_position = new(Random.Range(map_border.xMin, map_border.xMax), Random.Range(map_border.yMin, map_border.yMax));
            collider = Physics2D.OverlapCircle(GameManager.Instance.getGrid().CellToWorld(random_position), 0.08f);
            max_tries--;
        } while (collider != null);

        GameObject new_food = Instantiate(food);
        new_food.transform.position = GameManager.Instance.getGrid().GetCellCenterWorld(random_position);
        new_food.transform.parent = food_holder;
        new_food.name = random_position.ToString();
    }
}
