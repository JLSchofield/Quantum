using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour
{

    //position and speed parameters
    public Vector3 startPosition;
    public Vector3 targetPosition;
    float particleSpeed;

    //time parameters
    float startTime;


    void Update()
    {
        MoveToTarget();
    }

    //sets the starting time 
    public void SetParameters(Vector3 start, Vector3 target, float speed)
    {
        //set positions and speed
        startPosition = start;
        targetPosition = target;
        particleSpeed = speed;
        //reset time
        startTime = Time.time;
    }

    //moves the particle from startPosition to targetPosition
    void MoveToTarget()
    {
        //set the time elapsed
        float timeElapsed = Time.time - startTime;
        //calculate the time to complete the lerp
        float timeToComplete = Vector3.Distance(startPosition, targetPosition) * Time.deltaTime / particleSpeed;
        //lerp the position
        transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / timeToComplete);
    }

    //if the particle is at targetPosition, return true
    public bool HasArrived()
    {
        if(transform.position == targetPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //if collided with the player, destroy the object
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}