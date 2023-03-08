using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum State
    {
        Patrolling,
        Chasing,
        Attacking,
        Waiting,

    }
    
    State currentState;
    NavMeshAgent agent;

    public Transform[] destinationPoints;
    int destinationIndex;

    public Transform player;
    [SerializeField]
    float visionRange;
    [SerializeField]
    float attackRange;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Patrolling:
                Patrol();
            break;

            case State.Chasing:
                Chase();
            break;

            case State.Attacking:
                Attack();
            break;

            case State.Waiting:
                Wait();
            break;

            default:
                Chase();
            break;
            
        }
    }
  
    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position)<1)
        {
            
            
            if(destinationIndex < destinationPoints.Length)
            {
                destinationIndex += 1;
                currentState = State.Waiting;
            }
            if(destinationIndex == destinationPoints.Length)
            {
                destinationIndex = 0;     
            }
           
        }

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }

    }


    void Attack()
    {
        
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = State.Chasing;
        }

        
    }

    void Chase()
    {
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }
        
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            Debug.Log("Atacar");
            currentState = State.Attacking;            
        }
        
    }

    void Wait()
    {
        StartCoroutine("Esperar");
     
    }
    IEnumerator Esperar()
    {
        
        yield return new WaitForSeconds (5);
        currentState = State.Patrolling;
          
    }

    void OnDrawGizmos()
    {
        foreach (Transform point in destinationPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);

        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    

}
