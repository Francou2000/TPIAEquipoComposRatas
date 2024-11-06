using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindChaseState<StateEnum> : StateFollowPoints<StateEnum>
{
    private LineOfSight _los;
    private float _alertedLos;
    private float _alertedLosAngle;
    IMove _move;
    ISteering _steering;
    private IAlert _alert;
    public Node start;
    public Node goal;

    AudioSource _audioSource;
    DynamicBackgroundMusic _music;

    public EnemyPathfindChaseState(Transform entity, IMove move, ISteering steering, IAlert alert, LineOfSight los, float alertedLos, float alertedLosAngle, AudioSource audioSource, DynamicBackgroundMusic music, float distanceToPoint = 0.2F) : base(entity, distanceToPoint, move)
    {
        _move = move;
        _steering = steering;
        _los = los;
        _alertedLos = alertedLos;
        _alertedLosAngle = alertedLosAngle;
        _alert = alert;
        _audioSource = audioSource;
        _music = music;
    }

    public override void Enter()
    {
        _los.range = _alertedLos;
        _los.angle = _alertedLosAngle;
        _alert.IsAlerted = true;
        //actualizo el LoS

        _audioSource.Play();

        if (_music != null)
        {
            _music.SwitchToDangerMusic();
        }
        
        SetPathAStar();
    }
    
    public override void Execute()
    {
        base.Execute();
        SetPathAStar();
        //deprecated: ahora el movimiento está en StateFollowPoints
        //Vector3 dir = _steering.GetPoint();
        //_move.Move(dir.normalized);
        _alert.AlertedTimer += Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();

        if (_music != null)
        {
            _music.SwitchToNormalMusic();
        }
    }
    
    public void SetPathAStar()
    {
        var start = GetNearNode(_entity.position);
        
        //Uso el pursuit para determinar donde va a estar el objetivo
        goal = GetNearNode(_steering.GetPoint());
        
        List<Node> path = ASTAR.Run<Node>(start, IsSatisfies, GetConnections, GetCost, Heuristic);
        if (path.Count <= 0) return;
        SetWaypoints(GetPathVector(path));
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
