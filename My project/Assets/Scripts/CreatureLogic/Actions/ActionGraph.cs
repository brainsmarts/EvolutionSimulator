using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGraph
{

    public ActionNode root{get; private set;}
    List<ActionNode> nodes;
    public ActionGraph(ActionNode root, List<ActionNode> nodes)
    {
        this.root = root;
        this.nodes = nodes;
    }
}
