using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public Transform[] destinations;

    public float distanceToFollowPath;

    private int i = 0;

    private GameObject player;

    [Header("------- Follow Player --------")]

    public bool followPlayer;

    private float distanceToPlayer;

    public float distanceToFollowPlayer = 10f;


    void Start()
    {
        if (destinations == null || destinations.Length == 0)
        {
            transform.gameObject.GetComponent<AI>().enabled = false;
        }

        else
        {
            navMeshAgent.destination = destinations[0].transform.position;

            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        }        
    }


    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= distanceToFollowPlayer && followPlayer)
        {
            FollowPlayer();
        }
        else 
        {
            EnemyPath();
        }
    }


    public void EnemyPath()
    {
        navMeshAgent.destination = destinations[i].position;

        if (Vector3.Distance(transform.position, destinations[i].position) <= distanceToFollowPath) 
        { 
            if (destinations[i] != destinations[destinations.Length - 1])
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }
    }


    public void FollowPlayer()
    {
        navMeshAgent.destination = player.transform.position;
    }
}
