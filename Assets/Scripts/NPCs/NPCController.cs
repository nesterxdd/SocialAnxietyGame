using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    //Main settings for the NPC such as type and contents
    [Header("NPC Settings")]
    [SerializeField] private NpcType npcType;
    public MainNpcSO mainContents;
    public NPCContents contents;
    public bool talking;
    public bool waitingForUser;
    public GameObject indicatorIcon;
    private GameProgression gameProgress;
    [SerializeField] private UnityEvent _onConversationEnd;

    //The below are for NPCs with AI Pathing
    [Header("Movement")] 
    public NPCPath npcPath;
    private Coroutine lookAtPlayer;
    private Coroutine lookAtTarget;
    private NavMeshAgent agent;
    private bool moving;
    private bool returning;
    private bool waiting;
    private float waitTime;
    private float waitTimer;
    
    //Used for moving around the map
    [Header("Pathfinding")] 
    public Transform player;
    public Transform currentTarget;
    private int targetIndex;

    /// <summary>
    /// Called when player finishes conversation with main NPC.
    /// </summary>
    public UnityEvent OnConversationEnd => _onConversationEnd;

    private void Start()
    {
        gameProgress = GameProgression.instance;
        talking = false;
        waitingForUser = false;
        
        switch (npcType)
        {
            case NpcType.Static:
                contents.completed = false;
                break;
            
            case NpcType.Main:
                mainContents.completed = false;
                break;
            
            case NpcType.Moving:
                contents.completed = false;
                agent = GetComponent<NavMeshAgent>();
                targetIndex = 0;
                moving = true;
                returning = false;
                waiting = false;
                waitTime = npcPath.waitTimer;
                currentTarget = npcPath.points[0];
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void Update()
    {
        if (waitingForUser)
        {
            indicatorIcon.SetActive(true);
        }
        if (talking && indicatorIcon)
        {
            indicatorIcon.SetActive(false);
        }
        
        switch (npcType)
        {
            case NpcType.Static:
                break;
            
            case NpcType.Main:
                break;
            
            //AI Pathfinding
            case NpcType.Moving:
                if (talking)
                {
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    moving = false;
                }
                else
                {
                    moving = true;
                }
                if (moving)
                {
                    agent.isStopped = false;
                    if (agent.remainingDistance <= 5f && !waiting)
                    {
                        moving = false;
                        waiting = true;
                        waitTimer = waitTime;
                    }
                }
                else
                {
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                }
                if (waiting)
                {
                    waitTimer -= Time.deltaTime;
                    if (!(waitTimer < 0f)) return;
                    waiting = false;
                    ChangePoint();
                    moving = true;
                    SetDestination();
                }
                else if (moving && !waiting && !talking)
                {
                    SetDestination();
                }
                break;
            
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Move the NPC to the next point in the path.
    /// </summary>
    private void ChangePoint()
    {
        var length = npcPath.points.Length;
        if (!returning)
        {
            targetIndex++;
        }
        else
        {
            targetIndex--;
        }

        if (targetIndex < 0)
        {
            targetIndex = length - 1;
        }
        else if (targetIndex > length - 1)
        {
            targetIndex = 0;
        }
        
        currentTarget = npcPath.points[targetIndex];
    }

    /// <summary>
    /// Set the destination of the NPC to the current target.
    /// </summary>
    private void SetDestination()
    {
        agent.destination = currentTarget.position;
    }

    /// <summary>
    /// Make NPC look at the transform.
    /// </summary>
    private void LookAtTransform(Transform target)
    {
        if (lookAtTarget != null)
        {
            StopCoroutine(lookAtTarget);
        }

        lookAtTarget = StartCoroutine(TurnTowards(target));
    }

    /// <summary>
    /// Set the NPC to talking and look at the player
    /// </summary>
    public void StartConversation()
    {
        talking = true;
        LookAtTransform(player);
    }

    /// <summary>
    /// Set the NPC back to normal and look at its target
    /// </summary>
    public void EndConversation()
    {
        Debug.Log("Closed");
        talking = false;
        LookAtTransform(currentTarget);
    }

    /// <summary>
    /// Make NPC look at the given Transform.
    /// </summary>
    private IEnumerator TurnTowards(Transform turnTarget)
    {
        Quaternion lookRotation = Quaternion.LookRotation(turnTarget.position - transform.position);
        lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y,
            transform.rotation.eulerAngles.z);

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * 0.5f;
            yield return null;
        }
    }
    
    /// <summary>
    /// What type of NPC it is
    /// </summary>
    private enum NpcType
    {
        Static, Moving, Main
    }
}
