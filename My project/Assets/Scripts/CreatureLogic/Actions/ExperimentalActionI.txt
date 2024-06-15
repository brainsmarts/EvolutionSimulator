using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ExperimentalActionI
{
    public void SetRigidBody(Rigidbody2D rb);
    public void SetData(CreatureData data);
    public void SetScanner(RangeScanner scanner);
    public void SetNextActions();
    public bool StartCondition();
    public bool StopCondition();
    public void OnEnter();
    public void OnExit();   
    public void Run();
    public string ToString();
}
public class ActionGraphNode
{
    
    public ExperimentalActionI action;
    List<ActionGraphNode> next_actions;

    public ActionGraphNode(ExperimentalActionI action)
    {
        this.action = action;
        next_actions = new();
    }

    public void AddAction(ActionGraphNode new_action)
    {
        next_actions.Add(new_action);
    }

    public ActionGraphNode NextAction()
    {
        if (!action.StopCondition())
        {
            return null;
        }
        foreach(ActionGraphNode next_action in next_actions)
        {
            if (next_action.action.StartCondition())
            {
                return next_action;
            }
        }

        return null;
    }
}

public class ActionGraph
{

    ActionGraphNode root;
    List<ActionGraphNode> nodes;
    public ActionGraph(ActionGraphNode root, List<ActionGraphNode> nodes)
    {
        this.root = root;
        this.nodes = nodes;
    }
}
public class ActionController
{
    CreatureData Data;
    RangeScanner scanner;
    Rigidbody2D rb;
    Wander wander;
    Find_Food food;
    ActionGraph graph;
    ActionGraphNode current_action_node;
    void Start()
    {
        wander = new();
        food = new();
       
        ActionGraphNode wander_node = new ActionGraphNode(wander);
        ActionGraphNode findfood_node = new ActionGraphNode(food);
        wander_node.AddAction(findfood_node);
        findfood_node.AddAction(wander_node);
        List<ActionGraphNode> actions = new List<ActionGraphNode>();
        actions.Add(wander_node);
        actions.Add(findfood_node);
        graph = new ActionGraph(wander_node, actions);
    }

    void FixedUpdate()
    {
         ActionGraphNode next = current_action_node.NextAction();
        if(next != null)
        {
            current_action_node.action.OnExit();
            current_action_node = next;
            current_action_node.action.OnEnter();
        }

        current_action_node.action.Run();
        
    }
}

public class Wander : ExperimentalActionI
{
    public bool Condition()
    {
        throw new System.NotImplementedException();
    }

    public ExperimentalActionI NextAction()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void Run()
    {
        throw new System.NotImplementedException();
    }

    public void SetData(CreatureData data)
    {
        throw new System.NotImplementedException();
    }

    public void SetRigidBody(Rigidbody2D rb)
    {
        throw new System.NotImplementedException();
    }

    public void SetScanner(RangeScanner scanner)
    {
        throw new System.NotImplementedException();
    }
}

public class Find_Food : ExperimentalActionI
{
    public bool Condition()
    {
        throw new System.NotImplementedException();
    }

    public ExperimentalActionI NextAction()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void Run()
    {
        throw new System.NotImplementedException();
    }

    public void SetData(CreatureData data)
    {
        throw new System.NotImplementedException();
    }

    public void SetRigidBody(Rigidbody2D rb)
    {
        throw new System.NotImplementedException();
    }

    public void SetScanner(RangeScanner scanner)
    {
        throw new System.NotImplementedException();
    }
}
