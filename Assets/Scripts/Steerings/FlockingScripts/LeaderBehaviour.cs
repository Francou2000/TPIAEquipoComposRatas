using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public Transform target;
    public float timePrediction;
    public float multiplier;
    Seek _seek;
    Pursuit _pursuit;
    bool _isSeek;
    private Node goal;

    private List<Vector3> _waypoints;
    int _index;
    protected Transform _entity;
    float _distanceToPoint = 0.2f;
    bool _isFinishPath;
    IMove _move;
    
    
    private void Awake()
    {
        _seek = new Seek(transform, target);
        _pursuit = new Pursuit(transform, null, timePrediction);
        SetTarget(target);
        _entity = GetComponent<Transform>();
    }
    
    public Vector3 GetDir(List<IBoid> boids, IBoid self)
    {
        //Debug.Log("Entró en el run");
        if (_isFinishPath) return Vector3.zero;
        //Debug.Log("El camino no está finalizado");
        //Debug.Log("Índice: " + _index + ". Total: " + _waypoints.Count);
        if (_index > _waypoints.Count - 1) _index = 0; 
        Vector3 point = _waypoints[_index];
        point.y = _entity.position.y; //Horizontal move
        Vector3 dir = point - _entity.position;
        
        if (dir.magnitude < _distanceToPoint)
        {
            if (_index + 1 < _waypoints.Count)
                _index++;
            else
            {
                _isFinishPath = true;
                OnFinishPath();
                return Vector3.zero;
            }
        }

        return dir.normalized * multiplier;
        //_move.Move(dir.normalized);

        // OnMove(dir.normalized);
        /*if (target == null) return Vector3.zero;
        if (_isSeek) return _seek.GetDir() * multiplier;
        return _pursuit.GetDir() * multiplier;*/
    }

    public void SetPathAStar()
    {
        var start = GetNearNode(_entity.position);
        
        //Uso el pursuit para determinar donde va a estar el objetivo
        goal = GetNearNode(_pursuit.GetPoint());//<-
        
        List<Node> path = ASTAR.Run<Node>(start, IsSatisfies, GetConnections, GetCost, Heuristic);
        //Debug.Log("Nodos seteados: " + path.Count);
        if (path.Count <= 0) return;
        SetWaypoints(GetPathVector(path));
    }
    
    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null) return;
        target = newTarget;
        var rb = newTarget.GetComponent<Rigidbody>();
        if (rb)
        {
            _pursuit.Target = rb;
            _isSeek = false;
        }
        else
        {
            _seek.Target = newTarget;
            _isSeek = true;
        }
    }
    
    public void SetWaypoints(List<Vector3> newPoints)
    {
        if (newPoints.Count == 0) return;
        _waypoints = newPoints;
        _index = 1;
        _isFinishPath = false;
        OnStartPath();
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
        
        if (goal == null)
        {
            return 0;
        }

        float h = Vector3.Distance(node.transform.position, goal.transform.position);
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
    
    protected virtual void OnMove(Vector3 dir)
    {

    }

    protected virtual void OnStartPath()
    {

    }

    protected virtual void OnFinishPath()
    {
        
    }

    public bool IsFinishPath => _isFinishPath;
}
