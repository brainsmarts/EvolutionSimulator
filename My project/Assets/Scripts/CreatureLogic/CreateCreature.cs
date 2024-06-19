using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class CreateCreature : MonoBehaviour
{

    public static CreateCreature instance;
    [SerializeField] private Tilemap world_map;
    private BoundsInt map_border;
    [SerializeField] GameObject creature_prefab;
    [SerializeField] int NumOfStartingCreatures;

    void Start(){
        instance = this;
        id = 2;
        map_border = world_map.cellBounds;
        for(int i = 0; i < NumOfStartingCreatures; i++)
        {
            SpawnCreature();
        }
    }
    private int id;

    public GameObject creature_holder;

    //at x position
    public void SpawnCreature(){
        id++;
        int random_x = Random.Range(map_border.xMin, map_border.xMax);
        int random_y = Random.Range(map_border.yMin, map_border.yMax);
        Vector3 random_position = GameManager.Instance.getGrid().CellToWorld(new Vector3Int(random_x, random_y));
            
        GameObject creature = Instantiate(creature_prefab, random_position, Quaternion.identity);
        creature.transform.parent = creature_holder.transform;

        CreatureData data = new(id, 100, Random.Range(30,40), 8, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), creature.transform);
        BaseCreature baseCreature = creature.GetComponent<BaseCreature>();
        baseCreature.SetActions(CreateActions(creature.GetComponent<Rigidbody2D>(), data, creature.GetComponentInChildren<RangeScanner>()));
        
        baseCreature.SetData(data);
        SpriteRenderer spriteR = creature.GetComponent<SpriteRenderer>();
        spriteR.color = data.Color;
        //Instantiate(creature);
        //Debug.Log("Creature Created");

        creature.name = baseCreature.data.ID.ToString();
    }

    public void BreedNewCreature(CreatureData data1, CreatureData data2){
        id++;

        GameObject new_creature = Instantiate(creature_prefab, creature_holder.transform);
        BaseCreature creature_base = new_creature.GetComponent<BaseCreature>();

        CreatureData data3 = CreateData(data1, data2, new_creature.GetComponent<Rigidbody2D>(), new_creature.GetComponentInChildren<RangeScanner>());
        creature_base.SetData(data3);

        creature_base.SetActions(CreateActions(new_creature.GetComponent<Rigidbody2D>(),data3,new_creature.GetComponentInChildren<RangeScanner>()));
 
        SpriteRenderer spriteR = new_creature.GetComponent<SpriteRenderer>();
        spriteR.color = data3.Color;
        new_creature.name = creature_base.data.ID.ToString();
    }

    private CreatureData CreateData(CreatureData parent1, CreatureData parent2, Rigidbody2D creature_rb, RangeScanner scanner){
        CreatureData data;
        int min;
        int max;

        min = parent1.Energy < parent2.Energy ? parent1.Energy : parent2.Energy;
        max = parent1.Energy > parent2.Energy ? parent1.Energy : parent2.Energy;
        int energy = Random.Range(min-1, max + 1);

        min = parent1.Sight_range < parent2.Sight_range ? parent1.Sight_range : parent2.Sight_range;
        max = parent1.Sight_range > parent2.Sight_range ? parent1.Sight_range : parent2.Sight_range;
        int sight_range = Random.Range(min -1, max +1);

        min = parent1.Speed < parent2.Speed ? parent1.Speed : parent2.Speed;
        max = parent1.Speed  > parent2.Speed ? parent1.Speed : parent2.Speed;
        int speed = Random.Range(min -1, max +1);

        Color color = Color.Lerp(parent1.Color, parent2.Color, 1);
        data = new(id, energy, speed, sight_range, color, creature_rb.transform);
        return data;
    }

    private ActionGraph CreateActions(Rigidbody2D creature_rb, CreatureData data, RangeScanner scanner){
        
        FindFood find_food = new();
        InitAction(find_food, creature_rb, data, scanner);

        Wander wander = new();
        InitAction(wander, creature_rb, data, scanner);

        ActionNode find_food_node = new(find_food);
        ActionNode wander_node = new(wander);
        find_food_node.AddAction(wander_node);
        wander_node.AddAction(find_food_node);
        wander_node.AddAction(wander_node);
        List<ActionNode> action_list = new();
        action_list.Add(wander_node);
        action_list.Add(find_food_node);
        ActionGraph actions = new(wander_node, action_list);

        return actions;
    }

    private void InitAction(IAction action, Rigidbody2D creature_rb, CreatureData data, RangeScanner scanner){
        action.SetData(data);
        action.SetRigidBody(creature_rb);
        action.SetScanner(scanner);
    }
}
