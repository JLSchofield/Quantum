using UnityEngine;
using System.Collections;

//creates an attack based on fenymann diagrams

public class Feynmann : MonoBehaviour
{
    [SerializeField]
    GameObject leftHandGO;
    GameObject rightHandGO;

    [Header("Particles")]
    [SerializeField]
    GameObject[] particlePrefabs;
    GameObject particle1;
    GameObject particle2;

    [Header("Points")]
    Vector3 leftHand;
    Vector3 rightHand;
    Vector3 collisionPoint;
    Vector3 separationPoint;
    Vector3 leftDestructionPoint;
    Vector3 rightDestructionPoint;

    [Header("Parameters")]
    float particleTravelDistance;
    float bosonTravelDistance;
    float particleSpeed;

    enum FeynmannState
    {
        Create,
        MoveToCollision,
        MoveToSeparation,
        MoveToDestruction
    }
    FeynmannState currentState;

    void Start()
    {
        //get left and right hand positions
        leftHand = leftHandGO.transform.position;
        rightHand = rightHandGO.transform.position;
    }

    void Update()
    {
        CallState();
    }

    //sets collision, separation and destruction points
    void GetPoints()
    {
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
    }

    void CallState()
    {
        switch (currentState)
        {
            case FeynmannState.Create:
                GetPoints();
                Create();
                break;
            case FeynmannState.MoveToCollision:
                break;
            case FeynmannState.MoveToSeparation:
                break;
            case FeynmannState.MoveToDestruction:
                break;
        }
    }

    void Create()
    {      
        //create the initial two particles
        particle1 = (GameObject)Instantiate(particlePrefabs[0], leftHand, particlePrefabs[0].transform.rotation);
        particle2 = (GameObject)Instantiate(particlePrefabs[1], rightHand, particlePrefabs[1].transform.rotation);
        //rotate each particle to look at the collision point
        particle1.transform.LookAt(collisionPoint);
        particle2.transform.LookAt(collisionPoint);
        currentState = FeynmannState.MoveToCollision;
    }

    void MoveToCollision()
    {

    }

    IEnumerator MoveIEnum(Vector3 startPosition, Vector3 endPosition, float speed)
    {
        yield return 0;
    }
}