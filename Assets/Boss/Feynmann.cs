using UnityEngine;
using System.Collections;

//creates an attack based on fenymann diagrams

public class Feynmann : MonoBehaviour
{
    //Boss GOs
    [Header("Boss GOs")]
    [SerializeField]
    GameObject leftHandGO;
    [SerializeField]
    GameObject rightHandGO;

    //Particles
    [Header("Particles")]
    [SerializeField]
    GameObject[] particlePrefabs;
    GameObject particle1GO;
    GameObject particle2GO;
    Particle particle1;
    Particle particle2;
    GameObject bosonGO;
    Particle boson;

    //Particle parameters
    [SerializeField]
    float particleTravelDistance;
    [SerializeField]
    float bosonTravelDistance;
    [SerializeField]
    float particleSpeed;
    [SerializeField]
    float bosonSpeed;

    //Points
    Vector3 leftHand;
    Vector3 rightHand;
    Vector3 collisionPoint;
    Vector3 separationPoint;
    Vector3 leftDestructionPoint;
    Vector3 rightDestructionPoint;

    //delegate
    delegate void FeynmannState();
    FeynmannState currentState;

    void Start()
    {
        //get left and right hand positions
        leftHand = leftHandGO.transform.position;
        rightHand = rightHandGO.transform.position;

        //set delegate
        currentState = GetPoints;
    }

    void Update()
    {
        //if the current state is not null, do it
        if(currentState != null)
        {
            currentState();
        }
        //check if there are particles here
        CheckForParticles();
    }

    #region DELEGATE_FUNCTIONS
    //sets collision, separation and destruction points
    void GetPoints()
    {
        Debug.Log("Getting points...");
        //get the centre point between both hands
        Vector3 centrePoint = (leftHand + rightHand) / 2f;
        //get the forward direction of the boss
        Vector3 bossForwardDirection = transform.root.transform.forward;
        //get the distance between the hands
        float handsDistance = Vector3.Distance(leftHand, rightHand);
        //get the collision point such that the hands are both 60 degrees to it
        particleTravelDistance = Mathf.Sin(60f * Mathf.Deg2Rad) * handsDistance / 2f;
        collisionPoint = centrePoint + bossForwardDirection * particleTravelDistance;
        //get the separation point
        separationPoint = collisionPoint + bossForwardDirection * bosonTravelDistance;
        //get the left and right destruction points
        leftDestructionPoint = separationPoint + new Vector3(Mathf.Cos(60f * Mathf.Deg2Rad), 0f, Mathf.Sin(60f * Mathf.Deg2Rad)) * particleTravelDistance;
        rightDestructionPoint = separationPoint + new Vector3(-Mathf.Cos(60f * Mathf.Deg2Rad), 0f, Mathf.Sin(60f * Mathf.Deg2Rad)) * particleTravelDistance;

        Debug.Log("Got points.");
        //set delegate
        currentState = CreateFirstParticles;
    }

    void CreateFirstParticles()
    {
        Debug.Log("Creating first particles...");

        //create the initial two particles
        if (!particle1GO)
        {
            //instatiate the particle
            particle1GO = (GameObject)Instantiate(particlePrefabs[0]);
            //get the particle component
            particle1 = particle1GO.GetComponent<Particle>();
            //set the particles parameters
            particle1.SetParameters(leftHand, collisionPoint, particleSpeed);

            Debug.Log("Particle 1 created.");
        }
        if (!particle2GO)
        {
            //instatiate the particle
            particle2GO = (GameObject)Instantiate(particlePrefabs[0]);
            //get the particle component
            particle2 = particle2GO.GetComponent<Particle>();
            //set the particles parameters
            particle2.SetParameters(rightHand, collisionPoint, particleSpeed);

            Debug.Log("Particle 2 created.");
        }

        Debug.Log("Created first particles");

        //set delegate
        currentState = MoveToCollision;
    }

    void MoveToCollision()
    {
        //if both particles have arrived at their targetPositions
        if(particle1.HasArrived() || particle2.HasArrived())
        {
            Debug.Log("Both particles have arrived.");
            currentState = DestroyTwoParticles;
        }           
    }

    void DestroyTwoParticles()
    {
        Debug.Log("Destroying particles...");
        //destroy both particles if they exist
        if (particle1GO)
        {
            particle1GO.SetActive(false);
        }
        if (particle2GO)
        {
            particle2GO.SetActive(false);
        }

        Debug.Log("Destroyed particles.");
        //set delegate
        currentState = CreateBoson;
    }
    
    void CreateBoson()
    {
        Debug.Log("Creating boson...");
        //create the boson
        if (!bosonGO)
        {
            //instantiate the boson
            bosonGO = (GameObject)Instantiate(particlePrefabs[0]);
            //get the particle component
            boson = bosonGO.GetComponent<Particle>();
            //set the particle parameters
            boson.SetParameters(collisionPoint, separationPoint, bosonSpeed);

            Debug.Log("Created boson.");
        }
        
        //set delegate
        currentState = MoveToSeparation;
    }

    void MoveToSeparation()
    {
        //if there is a boson
        if (bosonGO)
        {
            //if that boson is at the separation point
            if (boson.HasArrived())
            {
                currentState = DestroyBoson;
            }
        }
    }

    void DestroyBoson()
    {
        //the boson is present
        if (bosonGO)
        {
            Debug.Log("Destroying boson...");
            //destroy the boson
            Destroy(boson);
            //set the particles to be active again
            if (particle1GO)
            {
                particle1GO.SetActive(true);
            }
            if (particle2GO)
            {
                particle2GO.SetActive(true);
            }
        }

        Debug.Log("Destroyed boson.");
        //set delegate
        currentState = CreateSecondParticles;
    }

    void CreateSecondParticles()
    {
        Debug.Log("Creating second particles...");
        //if particle 1 exists
        if (particle1GO)
        {
            particle1.SetParameters(separationPoint, leftDestructionPoint, particleSpeed);
        }
        //if particle 2 exists
        if (particle2GO)
        {
            particle2.SetParameters(separationPoint, rightDestructionPoint, particleSpeed);
        }

        Debug.Log("Created second particles.");
        //set delegate
        currentState = MoveToDestruction;
    }

    void MoveToDestruction()
    {
        //if both particles have arrived at their targetPositions
        if (particle1.HasArrived() || particle2.HasArrived())
        {
            Debug.Log("Both particles have arrived.");
            currentState = FinishFeynmann;
        }
    }

    void FinishFeynmann()
    {
        Debug.Log("Destroying particles...");
        if (particle1GO)
        {
            Debug.Log("Destroying particle 1.");
            Destroy(particle1);
        }

        if(particle2GO)
        {
            Debug.Log("Destroying particle 2.");
            Destroy(particle2);
        }

        Debug.Log("Destroyed particles.");
        //destroy this gameobject
        //Destroy(gameObject);
    }
    #endregion

    //checks if both particles are activate
    void CheckForParticles()
    {
        //if neither particle exists
        if (!particle1 && !particle2 && !boson)
        {
            Debug.Log("No particles, destroying game object");
            //destroy this gameobject
            Destroy(gameObject);
        }
    }

    //#region MOVEMENT_IENUMERATORS
    //IEnumerator MoveIEnum(GameObject go, Vector3 startPosition, Vector3 endPosition, float speed, FeynmannState nextDelegate)
    //{
    //    Debug.Log("Moving particle, end delegate: " + nextDelegate + " ...");
    //    float startTime = Time.time;
    //    float currentTime = startTime;
    //    //calculate the time it'll take to complete the lerp
    //    float timeToComplete = Vector3.Distance(startPosition, endPosition) * Time.deltaTime / speed;

    //    while(currentTime - startTime < timeToComplete)
    //    {
    //        //lerp between start and end
    //        go.transform.position = Vector3.Lerp(startPosition, endPosition, (currentTime - startTime) / timeToComplete);
    //        //increase time
    //        currentTime += Time.deltaTime;
    //        yield return 0;
    //    }

    //    //finished lerping, set the state
    //    Debug.Log("Moved particles");
    //    currentState = nextDelegate;
    //}
    //#endregion

}