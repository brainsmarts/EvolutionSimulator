using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode
{
    List<ActionNode> next_actions;
    public IAction action {get; private set;}
    public ActionNode(IAction action){
        next_actions = new();
        this.action = action;
    }
    public void AddAction(ActionNode new_action)
    {
        next_actions.Add(new_action);
    }
    public ActionNode NextAction()
    {
        if (!action.EndCondition())
        {
            return null;
        }
        foreach(ActionNode next_action in next_actions)
        {
            if (next_action.action.StartCondition())
            {
                return next_action;
            }
        }
        return null;
    }
}
