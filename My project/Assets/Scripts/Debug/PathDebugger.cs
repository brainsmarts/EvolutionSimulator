using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathDebugger : MonoBehaviour
{
    [SerializeField]
    Tile tile;
    [SerializeField]
    Tilemap map;
    [SerializeField]
    Tile cal_tile;
    [SerializeField]
    Tilemap calculation_map;
    [SerializeField]
    Tilemap world_map;
    Stack<Vector3Int> stack;
    private BoundsInt bounds;
    private int minx, miny, maxx, maxy;
    private int minabx, minaby, maxabx, maxaby;
    Vector3Int a;
    Vector3Int b;

    List<int[]> visited;
    List<int[]> prev;
    Queue<int> xq;
    Queue<int> yq;
    bool calculating;

    int[,] directions = new int[8, 2] {
            {1,0},
            {-1,0},
            {0,1},
            {0,-1},
            {1,1},
            {1,-1},
            {-1,1},
            {-1,-1}
        };
    // Start is called before the first frame update
    void Start()
    {
        //get random coordinates
        world_map.CompressBounds();
        bounds = world_map.cellBounds;
        minx = bounds.xMin; miny = bounds.yMin;
        maxx = bounds.xMax; maxy = bounds.yMax;
        SameXRandom();
        InitPathCalculator();
        
    }

    // Update is called once per frame
    void Update()
    {
        ResetPath();
    }

    private void FixedUpdate()
    {
        if (calculating)
        {
            CalculatePath();
            return;
        }
        if(stack.Count > 0)
        {
            map.SetTile(stack.Pop(), tile);
        }
    }

    private void ResetPath()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            calculation_map.ClearAllTiles();
            map.ClearAllTiles();
            CompletelyRandomPoints();
            InitPathCalculator();
        }
    }

    private void CompletelyRandomPoints()
    {
        a = new Vector3Int(Random.Range(minx, maxx), Random.Range(miny, maxy));
        b = new Vector3Int(Random.Range(minx, maxx), Random.Range(miny, maxy));
    }

    private void SameXRandom()
    {
        int x = Random.Range(minx, maxx);
        a = new Vector3Int(x, Random.Range(miny, maxy));
        b = new Vector3Int(x, Random.Range(miny, maxy));
    }

    private void InitPathCalculator()
    {
        ClearConsole();
        Debug.Log("Point A: " + a);
        Debug.Log("Point B: " + b);
        calculating = true;
        visited = new List<int[]>();
        prev = new List<int[]>();
        xq = new Queue<int>();
        yq = new Queue<int>();
        xq.Enqueue(a.x);
        yq.Enqueue(a.y);
        minabx = a.x - b.x > 0 ? b.x : a.x;
        minaby = a.y - b.y > 0 ? b.y : a.y;
        maxabx = a.x - b.x < 0 ? b.x : a.x;
        maxaby = a.y - b.y < 0 ? b.y : a.y;
    }

    private void CalculatePath()
    {
        if(xq.Count > 0) {
            int current_x = xq.Peek();
            int current_y = yq.Peek();
            //Debug.Log("From x , y " + current_x + ", " + current_y);

            for (int i = 0; i < 8; i++)
            {
                int[] neighboor = new int[2] { current_x + directions[i, 0], current_y + directions[i, 1] };

                //  Debug.Log(neighboor[0] + " " + neighboor[1]);
                if (InBounds(neighboor) && 
                    !GameManager.Instance.OutOfBounds(new Vector3Int(neighboor[0], neighboor[1]))
                    && !HasVisited(neighboor, visited)
                    && GameManager.Instance.IsNotRock(new Vector3Int(neighboor[0], neighboor[1])))
                {
                    calculation_map.SetTile(new Vector3Int(neighboor[0], neighboor[1]), cal_tile);
                    xq.Enqueue(neighboor[0]);
                    yq.Enqueue(neighboor[1]);
                    prev.Add(new int[] { current_x, current_y });
                    visited.Add(neighboor);
                    if (neighboor[0] == b.x && neighboor[1] == b.y)
                    {
                        
                        calculating = false;
                        GetPath(prev, visited, a, b);
                        return;
                    }
                }
                else
                {
                    //Debug.Log(neighboor[0] + " " + neighboor[1] + "Rejected");
                    //Debug.Log(GameManager.Instance.OutOfBounds(new Vector3Int(neighboor[0], neighboor[1])));
                }
            }

            xq.Dequeue();
            yq.Dequeue();
        }
        else
        {
            Debug.Log("No Path Found");
            calculating = false;
        }
    }

    private bool InBounds(int[] position)
    {
        //if in bounds
        if (position[0] >= minabx-1 && position[0] <= maxabx + 1 && position[1] >= minaby -1  && position[1] <= maxaby + 1)
        {
            //Debug.Log("In Bounds");
            return true;
        }

        return false;
    }


    public static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
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
    private void GetPath(List<int[]> prev, List<int[]> visited, Vector3Int a, Vector3Int b)
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
        stack = path;
        calculating = false;
    }
}
