using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScanner : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D range;
    private HashSet<BaseCreature> creatures_in_range;
    private HashSet<FoodScript> food_in_range;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("Start method called, initializing HashSets.");
        creatures_in_range = new();
        food_in_range = new();
        //range.enabled = false;
        range.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Creature"))
        { 
            creatures_in_range.Add(other.gameObject.GetComponent<BaseCreature>());  
        } else if (other.tag.Equals("Food"))
        {
            food_in_range.Add(other.gameObject.GetComponent<FoodScript>());
        }
           
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Creature"))
        {
            creatures_in_range.Remove(collision.GetComponent<BaseCreature>());
        }
        else if(collision.tag.Equals("Food"))
        {
            //Debug.Log("Food Removed");
            food_in_range.Remove(collision.gameObject.GetComponent<FoodScript>());
        }
    }

    public void SetRange(int creature_range)
    {
        range.radius = range.radius * creature_range;   
    }

    private bool CanSee(Collider2D see)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, see.transform.position - transform.position, Vector2.Distance(transform.position, see.transform.position));
        return hit.collider.Equals(see);
    }

    public HashSet<BaseCreature> GetCreatures()
    {
        if(creatures_in_range == null)
        {
            Debug.Log("Null HashSet");
        }
        return creatures_in_range;
    }

    public FoodScript GetNearestFood()
    {
        float distance = float.MaxValue;
        FoodScript nearest_food = null;
        foreach (FoodScript food in food_in_range)
        {
            //Debug.Log(distance);
            //Debug.Log(Vector2.Distance(food.GetPosition(), transform.position));
            if(Vector2.Distance(food.GetPosition(), transform.position) < distance)
            {
                nearest_food = food; 
            }
        }

        return nearest_food;
    }

    public void Enable()
    {
        range.enabled = true;
    }
}