using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GenericMovement
{
    //Get pathway from a to b
    public static Stack<Vector3Int> MoveTo(Vector3Int a, Vector3Int b){
        if (a.Equals(b))
            return new();

        int[,] directions = new int[4, 2] {
            {1,0},
            {-1,0},
            {0,1},
            {0,-1}
        };
        List<int[]> visited = new List<int[]>();
        List<int[]> prev = new List<int[]>();
        Queue<int> xq = new Queue<int>();
        Queue<int> yq = new Queue<int>();
        int minx = a.x - b.x > 0 ? b.x : a.x;
        int miny = a.y - b.y > 0 ? b.y : a.y;
        int maxx = a.x - b.x < 0 ? b.x : a.x;
        int maxy = a.y - b.y < 0 ? b.y : a.y;

        //Debug.Log("Point A: " + a);
        //Debug.Log("Point B: " + b);
        xq.Enqueue(a.x);
        yq.Enqueue(a.y);
        while (xq.Count > 0)
        {
            int current_x = xq.Peek();
            int current_y = yq.Peek();
            //Debug.Log("From x , y " + current_x + ", " + current_y);

            for(int i = 0; i < 4; i++)
            {
                int[] neighboor = new int[2] { current_x + directions[i, 0], current_y + directions[i, 1] };

                //  Debug.Log(neighboor[0] + " " + neighboor[1]);
                if (InBounds(neighboor, minx, miny, maxx, maxy) &&
                    !GameManager.Instance.OutOfBounds(new Vector3Int(neighboor[0], neighboor[1]))
                    && !HasVisited(neighboor, visited) 
                    && GameManager.Instance.IsNotRock(new Vector3Int(neighboor[0] , neighboor[1])))
                {
                    //Debug.Log("Neighboor Added " + neighboor[0] + ", " + neighboor[1]);
                    xq.Enqueue(neighboor[0]);
                    yq.Enqueue(neighboor[1]);
                    prev.Add(new int[] {current_x, current_y});
                    visited.Add(neighboor);
                    if (neighboor[0] == b.x && neighboor[1] == b.y)
                    {
                        return GetPath(prev, visited, a, b); ;
                    }
                }
            }

            xq.Dequeue();
            yq.Dequeue();
        }

        Debug.Log("No Path Found");
        return new();
       
    }

    private static bool InBounds(int[] position, int minx, int miny, int maxx, int maxy)
    {
        //if in bounds
        if (position[0] >= minx -1 && position[0] <= maxx +1 && position[1] >= miny && position[1] <= maxy)
        {
            //Debug.Log("In Bounds");
            return true;
        }
            
        return false;
    }

    private static bool HasVisited(int[] node, List<int[]> list)
    {
        foreach (int[] visited in list)
        {
            if (node[0] == visited[0] && visited[1] == node[1])
                return true;
        }
        return false;
    }
    private static Stack<Vector3Int> GetPath(List<int[]> prev, List<int[]> visited, Vector3Int a, Vector3Int b)
    {
        //find b
        Stack<Vector3Int> path = new();
        int[] last = prev.LastOrDefault();
        path.Push(b);
        path.Push(new Vector3Int(last[0], last[1]));
        //Debug.Log(last[0] + ", " + last[1] +" Is it equal to " + a);
        //do while last != a
        while (last[0] != a.x || last[1] != a.y)
        {
            //Debug.Log("Finding " + last);
            foreach (int[] visit in visited)
            {
                if (last[0] == visit[0] && last[1] == visit[1])
                {
                    //get index of visit

                    //Debug.Log(visit[0] + " , " + visit[1] +" Pushed");

                    int index = visited.IndexOf(visit);

                    last = prev[index];
                    path.Push(new Vector3Int(last[0], last[1]));
                    break;
                }

            }

            //Debug.Log("Last is now " + last[0] + " , " + last[1]);
        }
        if (path.Peek() == null)
        {
            Debug.Log("Returning Null Stack???");
        }
        return path;
    }
}
