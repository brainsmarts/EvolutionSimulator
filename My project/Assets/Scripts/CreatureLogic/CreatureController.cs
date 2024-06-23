using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BaseCreature : MonoBehaviour
{

    [SerializeField]
    public CreatureData data;
    [SerializeField]
    private RangeScanner scanner;

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    Collider2D collider;
    
    //private List<ActionBase> actions;
    public ActionGraph graph {get; private set;}
    public ActionNode current_action_node { get; private set; }


    [SerializeField]
    private float metobolism_rate = 10;
    private float metobolism_timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        scanner.SetRange(data.Sight_range);
        data.Target = null;
        scanner.Enable();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        DoAction();
        ObstacleCheck();
        CheckMetobolism();
        CheckDeath();
    }

    private void CheckMetobolism()
    {
        if (metobolism_timer > metobolism_rate)
        {
            data.DecreaseEnergy(1);
            metobolism_timer = 0;
        }
        else
        {
            metobolism_timer += Time.deltaTime; 
        }
    }
    private void DoAction()
    {
        ActionNode next = current_action_node.NextAction();
        if(next != null)
        {
            current_action_node.action.OnExit();
            current_action_node = next;
            current_action_node.action.OnEnter();
        }

        current_action_node.action.Run();
    }

    /* Obstacle Check
     * Purpose -> checks for any obstacles in the current direction the creature is facing
     * How it works
     * casts a raycast in the direction the creature is facing with x length
     * case 1: if the raycast detects an object other than itself
     *  cancel the current action, and force a new action
     *  if no actions can be forced, then the root action (likely idle) will be the new action
     * case 2:
     *  continue the current action
     */ 
    private void ObstacleCheck(){

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, rb.velocity, .16f);  
        Vector3 rayDirection = rb.velocity.normalized * .16f;

        //For Us to see where the ray is being cast
        Debug.DrawLine(rb.position, transform.position + rayDirection, Color.red);

        foreach (RaycastHit2D hit in hits)
        {

            if (!hit.collider.gameObject.Equals(gameObject))
            {
                Debug.Log("Obstacle Found: " + hit.collider);
                ActionNode next = current_action_node.ForceNextAction();
                //if no action is found, go back to the root
                if(next == null)
                {
                    next = graph.root;
                }
                current_action_node = next;
                current_action_node.action.OnEnter();
            }
        }
    }

    private void CheckDeath()
    {
        if (data.Current_energy <= 0)
        {
            Debug.Log("I Died");
            Destroy(gameObject);
        } 
    }

    public void SetData(CreatureData data){
        this.data = data;
    }

    public void SetActions(ActionGraph graph){
        Debug.Log("Setting Graph");
        this.graph = graph;
        current_action_node = graph.root;
        current_action_node.action.OnEnter();
    }

    public int GetAge()
    {
        return (int)((Time.time - data.TimeBorn) / 60); 
    }
    
    public Transform GetTransform() { return transform; }

    private void OnMouseDown()
    {
        DebugManager.Instance.Display(this);
    }
}
