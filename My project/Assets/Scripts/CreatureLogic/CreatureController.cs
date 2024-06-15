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
        MoveToTargetLocation();
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

    private void MoveToTargetLocation(){
        //Debug.Log(grid.WorldToCell(transform.position) + " < > " + grid.WorldToCell(data.Target_Location));
        
        //Condition comparing if distance is less than 0.01 is sketchy
        if (Vector3.Distance(transform.position,data.Target_Location) < 0.08f){
            if(data.path.Count > 0){
                data.NextInPath();
            } else{
                rb.velocity *= 0;
            }

        }

        rb.velocity = new Vector2(data.Target_Location.x - transform.position.x, data.Target_Location.y - transform.position.y).normalized * data.Speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity, .2f);
        //it hits its own collider first which is why it can never detect anything else
        Debug.DrawRay(transform.position, rb.velocity.normalized * 0.1f, Color.red);

        if (hit.collider != null && !collider.gameObject.Equals(gameObject))
        {
            Debug.Log("Stuck");
            float newx, newy;
            newx = -rb.velocity.y;
            newy = rb.velocity.x;
            rb.AddForce(new Vector2(newx, newy).normalized, ForceMode2D.Impulse);
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
