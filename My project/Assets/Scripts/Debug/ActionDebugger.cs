using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDebugger : MonoBehaviour
{
    private CreatureData data;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private RangeScanner scanner;
    public ActionGraph graph { get; private set; }
    public ActionNode current_action_node { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        //Creating Randomized Data
        data = new(1001, 100, Random.Range(30, 40), 8, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), transform);

        //Setting the action tree
        CreateActions();
        current_action_node = graph.root;
        current_action_node.action.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoAction();
        }
    }

    private void DoAction()
    {
        ActionNode next = current_action_node.NextAction();
        if (next != null)
        {
            current_action_node.action.OnExit();
            current_action_node = next;
            current_action_node.action.OnEnter();
            Debug.Log("Next Action Started: " + current_action_node.action.ToString());
        }

        current_action_node.action.PrintStatus();
        current_action_node.action.Run();
    }
    private void CreateActions()
    {
        FindFood find_food = new();
        InitAction(find_food);

        Wander wander = new();
        InitAction(wander);

        ActionNode find_food_node = new(find_food);
        ActionNode wander_node = new(wander);
        find_food_node.AddAction(wander_node);
        wander_node.AddAction(find_food_node);
        wander_node.AddAction(wander_node);
        List<ActionNode> action_list = new();
        action_list.Add(wander_node);
        action_list.Add(find_food_node);
        graph = new(wander_node, action_list);
    }

    private void InitAction(IAction action)
    {
        action.SetData(data);
        action.SetRigidBody(rb);
        action.SetScanner(scanner);
    }
}
