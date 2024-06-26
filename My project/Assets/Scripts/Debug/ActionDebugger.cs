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
    public ActionNode currentActionNode { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        //Creating Randomized Data
        data = new(1001, 100, Random.Range(30, 40), 8, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), transform);

        //Setting the action tree
        CreateActions();
        currentActionNode = graph.root;
        currentActionNode.action.OnEnter();
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
        ActionNode next = currentActionNode.NextAction();
        if (next != null)
        {
            currentActionNode.action.OnExit();
            currentActionNode = next;
            currentActionNode.action.OnEnter();
            Debug.Log("Next Action Started: " + currentActionNode.action.ToString());
        }

        currentActionNode.action.PrintStatus();
        currentActionNode.action.Run();
    }
    private void CreateActions()
    {
        FindFood findFood = new();
        InitAction(findFood);

        Wander wander = new();
        InitAction(wander);

        ActionNode findFoodNode = new(findFood);
        ActionNode wanderNode = new(wander);
        findFoodNode.AddAction(wanderNode);
        wanderNode.AddAction(findFoodNode);
        wanderNode.AddAction(wanderNode);
        List<ActionNode> actionList = new();
        actionList.Add(wanderNode);
        actionList.Add(findFoodNode);
        graph = new(wanderNode, actionList);
    }

    private void InitAction(IAction action)
    {
        action.SetData(data);
        action.SetRigidBody(rb);
        action.SetScanner(scanner);
    }
}
