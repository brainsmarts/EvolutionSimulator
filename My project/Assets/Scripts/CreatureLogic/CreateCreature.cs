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
    [SerializeField] private Tilemap worldMap;
    private BoundsInt mapBorder;
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] int numOfStartingCreatures;

    void Start(){
        instance = this;
        id = 2;
        mapBorder = worldMap.cellBounds;
        for(int i = 0; i < numOfStartingCreatures; i++)
        {
            SpawnCreature();
        }
    }
    private int id;

    public GameObject creatureHolder;

    //at x position
    public void SpawnCreature(){
        id++;
        int randomx = Random.Range(mapBorder.xMin, mapBorder.xMax);
        int randomy = Random.Range(mapBorder.yMin, mapBorder.yMax);
        Vector3 random_position = GameManager.Instance.getGrid().CellToWorld(new Vector3Int(randomx, randomy));
            
        GameObject creature = Instantiate(creaturePrefab, random_position, Quaternion.identity);
        creature.transform.parent = creatureHolder.transform;

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

        GameObject newCreature = Instantiate(creaturePrefab, creatureHolder.transform);
        BaseCreature creatureBase = newCreature.GetComponent<BaseCreature>();

        CreatureData data3 = CreateData(data1, data2, newCreature.GetComponent<Rigidbody2D>(), newCreature.GetComponentInChildren<RangeScanner>());
        creatureBase.SetData(data3);

        creatureBase.SetActions(CreateActions(newCreature.GetComponent<Rigidbody2D>(),data3,newCreature.GetComponentInChildren<RangeScanner>()));
 
        SpriteRenderer spriteR = newCreature.GetComponent<SpriteRenderer>();
        spriteR.color = data3.Color;
        newCreature.name = creatureBase.data.ID.ToString();
    }

    private CreatureData CreateData(CreatureData parent1, CreatureData parent2, Rigidbody2D creature_rb, RangeScanner scanner){
        CreatureData data;
        int min;
        int max;

        min = parent1.Energy < parent2.Energy ? parent1.Energy : parent2.Energy;
        max = parent1.Energy > parent2.Energy ? parent1.Energy : parent2.Energy;
        int energy = Random.Range(min-1, max + 1);

        min = parent1.SightRange < parent2.SightRange ? parent1.SightRange : parent2.SightRange;
        max = parent1.SightRange > parent2.SightRange ? parent1.SightRange : parent2.SightRange;
        int sight_range = Random.Range(min -1, max +1);

        min = parent1.Speed < parent2.Speed ? parent1.Speed : parent2.Speed;
        max = parent1.Speed  > parent2.Speed ? parent1.Speed : parent2.Speed;
        int speed = Random.Range(min -1, max +1);

        Color color = Color.Lerp(parent1.Color, parent2.Color, 1);
        data = new(id, energy, speed, sight_range, color, creature_rb.transform);
        return data;
    }

    private ActionGraph CreateActions(Rigidbody2D creature_rb, CreatureData data, RangeScanner scanner){
        
        FindFood findFood = new();
        InitAction(findFood, creature_rb, data, scanner);

        Wander wander = new();
        InitAction(wander, creature_rb, data, scanner);

        ActionNode findFoodNode = new(findFood);
        ActionNode wanderNode = new(wander);
        findFoodNode.AddAction(wanderNode);
        wanderNode.AddAction(findFoodNode);
        wanderNode.AddAction(wanderNode);
        List<ActionNode> action_list = new();
        action_list.Add(wanderNode);
        action_list.Add(findFoodNode);
        ActionGraph actions = new(wanderNode, action_list);

        return actions;
    }

    private void InitAction(IAction action, Rigidbody2D creature_rb, CreatureData data, RangeScanner scanner){
        action.SetData(data);
        action.SetRigidBody(creature_rb);
        action.SetScanner(scanner);
    }
}
