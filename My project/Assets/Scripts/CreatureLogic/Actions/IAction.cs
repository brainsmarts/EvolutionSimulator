using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction 
{
    public void SetRigidBody(Rigidbody2D rb);
    public void SetData(CreatureData data);
    public void SetScanner(RangeScanner scanner);
    public bool StartCondition();
    public bool EndCondition();
    public void OnEnter();
    public void OnExit();
    public void Run();
    public string ToString();
}
