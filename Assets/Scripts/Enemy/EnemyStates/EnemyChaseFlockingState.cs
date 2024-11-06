using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseFlockingState<StateEnum> : State<StateEnum>
{
    private LineOfSight _los;
    private float _alertedLos;
    private float _alertedLosAngle;
    IMove _move;
    private FlockingManager _flocking;
    private IAlert _alert;
    public Node start;
    public Node goal;

    AudioSource _audioSource;
    DynamicBackgroundMusic _music;

    public EnemyChaseFlockingState(Transform entity, IMove move, FlockingManager flocking, IAlert alert, LineOfSight los, float alertedLos, float alertedLosAngle, AudioSource audioSource, DynamicBackgroundMusic music, float distanceToPoint = 0.2F)
    {
        _move = move;
        _flocking = flocking;
        _los = los;
        _alertedLos = alertedLos;
        _alertedLosAngle = alertedLosAngle;
        _alert = alert;
        _audioSource = audioSource;
        _music = music;
    }

    public override void Enter()
    {
        base.Enter();
        _los.range = _alertedLos;
        _los.angle = _alertedLosAngle;
        _alert.IsAlerted = true;
        //actualizo el LoS

        _audioSource.Play();

        if (_music != null)
        {
            _music.SwitchToDangerMusic();
        }
        
        _flocking.Leader.SetPathAStar();
    }
    
    public override void Execute()
    {
        base.Execute(); 
        
        if (_flocking == null) return;
        var dir = _flocking.GetDir();
        _move.Move(dir);
        _move.Look(dir);
        
        _flocking.Leader.SetPathAStar();
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
    
    /*public void SetPathAStar()
    {
        var start = GetNearNode(_entity.position);
        
        //Uso el pursuit para determinar donde va a estar el objetivo
        goal = GetNearNode(_steering.GetPoint());
        
        List<Node> path = ASTAR.Run<Node>(start, IsSatisfies, GetConnections, GetCost, Heuristic);
        //Debug.Log("Nodos seteados: " + path.Count);
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
    }*/
}
