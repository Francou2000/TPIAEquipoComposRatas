using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePathfinding<T> : StateFollowPoints<T>
{
    IMove _move;
    public Node start;
    public Node goal;

    public StatePathfinding(Transform entity, IMove move, float distanceToPoint = 0.2F) : base(entity, distanceToPoint, move)
    {
        _move = move;
    }

    protected override void OnMove(Vector3 dir)
    {
        base.OnMove(dir);
        _move.Move(dir);
        _move.Look(dir);
    }

    protected override void OnStartPath()
    {
        base.OnStartPath();
    }

    protected override void OnFinishPath()
    {
        base.OnFinishPath();
    }
    
    public void SetPathAStar()
    {
        var start = GetNearNode(_entity.position);
        List<Node> path = ASTAR.Run<Node>(start, IsSatisfies, GetConnections, GetCost, Heuristic);
        if (path.Count <= 0) return;
        SetWaypoints(GetPathVector(path));
    }
    
    Vector3 GetPoint(Vector3 point)
    {
        return Vector3Int.RoundToInt(point);
    }
    
    Node GetNearNode(Vector3 pos)
    {
        var colls = Physics.OverlapSphere(pos, Constants.nearNodeDistance, Constants.nodeMask);
        Node nearNode = null;
        float nearDistance = 0;
        for (int i = 0; i < colls.Length; i++)
        {
            var currentNode = colls[i].GetComponent<Node>();
            if (currentNode == null) continue;

            var currentDistance = Vector3.Distance(currentNode.transform.position, pos);
            if (nearNode == null || nearDistance > currentDistance)
            {
                Vector3 dir = currentNode.transform.position - pos;
                if (Physics.Raycast(pos, dir.normalized, dir.magnitude, Constants.obsMask)) continue;

                nearNode = currentNode;
                nearDistance = currentDistance;
            }
        }
        return nearNode;
    } 

    float Heuristic(Node node)
    {
        float h = 0;
        h += Vector3.Distance(node.transform.position, goal.transform.position);
        return h;
    }

    float GetCost(Node parent, Node child)
    {
        float cost = 0;
        cost += Vector3.Distance(parent.transform.position, child.transform.position);
        return cost;
    }

    List<Vector3> GetPathVector(List<Node> path)
    {
        List<Vector3> pathVector = new List<Vector3>();
        for (int i = 0; i < path.Count; i++)
        {
            pathVector.Add(path[i].transform.position);
        }
        return pathVector;
    }

    bool IsSatisfies(Node current)
    {
        return current == goal;
    }

    List<Node> GetConnections(Node current)
    {
        return current.neightbourds;
    }
}