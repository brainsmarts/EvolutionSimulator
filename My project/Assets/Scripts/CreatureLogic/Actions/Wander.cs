using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Wander : IAction
{
    CreatureData data;
    Rigidbody2D rb;
    RangeScanner scanner;
    bool wandering = false;
    Vector3Int wander_target;
    Grid grid;

    public Wander()
    {
        grid = GameManager.Instance.getGrid();
    }

    public bool EndCondition()
    {
        if (Vector3.Distance(rb.position, grid.CellToWorld(wander_target)) < 0.08){
            wandering = false;
            return true;
        }
        return false;
    }

    public void OnEnter()
    {
        wander_target = data.SetRandomPath();
        while(wander_target == Vector3Int.zero){
            wander_target = data.SetRandomPath();
        }
        wandering = true;
    }

    public void OnExit()
    {
    }

    public void Run()
    {
        //do nothing
    }

    public void SetData(CreatureData data)
    {
        this.data = data;
    }

    public void SetRigidBody(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void SetScanner(RangeScanner scanner)
    {
        this.scanner = scanner;
    }

    public bool StartCondition()
    {
        return !wandering;   
    }

    override
    public string ToString(){
        return "Wander";
    }

    public void PrintStatus()
    {
        Debug.Log(this.ToString());
        Debug.Log("Target Location: " + wander_target.ToString());
        Debug.Log("Distance: " + Vector3.Distance(rb.position, grid.CellToWorld(wander_target)));
    }
}
