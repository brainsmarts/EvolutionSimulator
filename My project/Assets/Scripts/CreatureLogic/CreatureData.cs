using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
[Serializable]

public class CreatureData
{
    public Stack<Vector3Int> path { get; private set; }
    public Vector3 TargetLocation{get; set;}
    public Transform transform { get; }
    [SerializeField]
    public int ID {get;}
    [SerializeField]
    public int Energy {get;}
    [SerializeField]
    public int CurrentEnergy {get; private set;}
    [SerializeField]
    public int SightRange {get;}
    [SerializeField]
    public int Speed { get; private set;}
    public BaseCreature Target {get; set;}
    public float TimeBorn {get; private set;}
    public Color Color { get; private set;}

    private Grid grid;

    public CreatureData(int ID, int energy, int speed, int sight_range, Color color, Transform transform)
    {
        this.ID = ID;
        this.Energy = energy;
        this.Speed = speed;
        this.SightRange = sight_range;
        CurrentEnergy = energy / 2;
        TimeBorn = Time.time;
        Color = color;
        this.transform = transform;
        grid = GameManager.Instance.getGrid();
    }

    public void DecreaseEnergy(int amount){
        //Debug.Log("Decreasing Energy" + ID);
        CurrentEnergy -= amount;
    }

    public void IncreaseEnergy(int amount){
        CurrentEnergy += amount;
        if (CurrentEnergy > Energy) {
            CurrentEnergy = Energy;
        }
    }

    //returns a bool based on if the energy capacity is full or not
    public bool IsFull()
    {
        return CurrentEnergy >= Energy;
    }

    public void SetNewTargetLocation(Vector3Int new_location)
    {
        TargetLocation = new_location;
    }

    /* Sets a random target coordinate
     * If the position is out of bounds, or is a rock then get a new position
     */
    public Vector3Int SetRandomPath(){
        int negativex = UnityEngine.Random.Range(0f,1f) > .5f ? -1 : 1;
        int negativey = UnityEngine.Random.Range(0f,1f) > .5f ? -1 : 1;
        Vector3Int og_position = grid.WorldToCell(transform.position);
        Vector3Int position = grid.WorldToCell(transform.position);

        do
        {
            position.x = og_position.x + negativex * UnityEngine.Random.Range(5, 10);
            position.y = og_position.y + negativey * UnityEngine.Random.Range(5, 10);
        } while (GameManager.Instance.OutOfBounds(position) || !GameManager.Instance.IsNotRock(position));

        SetNewTargetLocation(position);
        return position;
    }
}
