using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int energyStored;

    [SerializeField]
    private HashSet<BaseCreature> creaturesInRange;
    public int EatFood()
    {
        Destroy(gameObject);
        return energyStored;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool InRange(Rigidbody2D creature)
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, 0.16f);
        foreach(Collider2D collider in results)
        {
            if (collider.CompareTag("Creature"))
            {
                if(collider.attachedRigidbody == creature)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
