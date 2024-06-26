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
    /* Next Action 
     * finds if the endcondition of the current action is done
     * case 1: if end condition true then find the next possible action
     * case 2: if not, return null
     * case 3: end condition is met but no possible next actions, return null
     */
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

    /* Force Next Action
     * checks if there is a next available action without checking the current action
     * if no possible actions, null is returned
     */
    public ActionNode ForceNextAction()
    {
        action.OnExit();
        foreach (ActionNode next_action in next_actions)
        {
            if (next_action.action.StartCondition())
            {
                return next_action;
            }
        }
        return null;
    }
}
