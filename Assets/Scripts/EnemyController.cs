using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    public float DistanceToActivate;
    public float Speed;
    public float HealthPoints = 10f;

    private float distanceToPlayer;
    private Vector3 patrolPointA;
    private Vector3 patrolPointB;
    private Vector3 currentDestination;
    private bool collisionDetected = false;
    private bool changeDirection = false;

    

    void Awake()
    {
        setPatrolPoints();
        currentDestination = patrolPointA;
        getPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);

        if (distanceToPlayer < DistanceToActivate)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * Speed / 2);
        }
        else
        {
            checkIfPatrolPointReached();
            moveEnemy();

        }

    }


    private void setPatrolPoints()
    {
        Vector3 randomVectorA = Random.insideUnitSphere * 10;
        Vector3 randomVectorB = Random.insideUnitSphere * 10;

        patrolPointA = new Vector3(transform.position.x + randomVectorA.x, transform.position.y, transform.position.z + randomVectorA.y);
        patrolPointB = new Vector3(transform.position.x - randomVectorB.x, transform.position.y, transform.position.z - randomVectorB.y);
    }

    private void checkIfPatrolPointReached()
    {
        float distanceToDestination = Vector3.Distance(currentDestination, transform.position);

        if (Mathf.Abs(distanceToDestination) < 0.5f || collisionDetected)
        {
            changeDirection = true;
            collisionDetected = false;
        }
        else
        {
            changeDirection = false;
        }
    }
    public void getPlayerObject()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Player = players[0];
    }

    private void moveEnemy()
    {
        if (!changeDirection)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, Time.deltaTime * Speed);
        }
        else
        {
            if (currentDestination == patrolPointA)
            {
                currentDestination = patrolPointB;
            }
            else
            {
                currentDestination = patrolPointA;
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            collisionDetected = true;
            setPatrolPoints();
        }
    }
}
