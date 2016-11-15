using UnityEngine;
using System.Collections;

//class for the quantum mechanics boss
//boss has 4 abilities
//  1. fire a feynman diagram attack
//  2. turn invisible, appear next to player and attack
//  3. create several mirages of the boss around arena, all charge up lasers, hit before firing
//  4. phase through ground, appear below enemy

public class Quantum : MonoBehaviour
{
    //enum to handle boss states
    public enum BossState
    {
        //boss is idle
        Idle,
        //boss moves around before using an attack
        Moving,
        //feynmann
        Feynmann,
        //invisible
        Invisible,
        //Mirage
        Mirage,
        //tunneling
        Tunnelling,
        //dodging attacks
        Dodging,
        //hurt, stunned
        Stunned
    }

    //enum for boss states
    BossState bossState;
    [Header("Idle")]
    [SerializeField]
    float idleTime;

    [Header("Moving")]
    [SerializeField]
    Vector3 arenaCentre;
    [SerializeField]
    float movingRadius;
    [SerializeField]
    float movingHeight;
    [SerializeField]
    float movingSpeed;
    [SerializeField]
    float movingTime;

    [Header("Feynmann")]
    GameObject[] createdBlasts;


    // Use this for initialization
    void Start()
    {
        //initially set state to be idle
        bossState = BossState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //change delegate depending on bossState if not changed
        CallState();
    }

    //sets the delegate function currentBossState
    void CallState()
    {
        switch (bossState)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.Moving:
                Moving();
                break;
            case BossState.Feynmann:
                Feynmann();
                break;
            case BossState.Invisible:
                Invisible();
                break;
            case BossState.Mirage:
                Mirage();
                break;
            case BossState.Tunnelling:
                Tunnelling();
                break;
            case BossState.Dodging:
                Dodging();
                break;
            case BossState.Stunned:
                Stunned();
                break;
        }
    }

    #region BOSS_STATE_FUNCTIONS
    //boss is idle
    void Idle()
    {
        //play idle animation
        //play idle sound

        //stay idle for a idleTime seconds
        StartCoroutine(IdleIEnum());
    }

    //boss moves around before using an attack
    void Moving()
    {
        //play moving animation
        //play moving sound

        //rotate around arena centre
        StartCoroutine(MovingIEnum());
    }

    //feynmann
    void Feynmann()
    {
        //fire blast
        //move
        //fire blast
        //move
        //fire blast
        //move
        //idle
    }

    //invisible
    void Invisible()
    {
        //invisible
        //select point on circle around player
        //visible
        //fire laser
    }

    //Mirage
    void Mirage()
    {
        //create several copies
        //move to edge of arena
        //charge laser
        //fire laser
    }

    //tunneling
    void Tunnelling()
    {
        //phase down into floor
        //get player point after time
        //wait a short amount of time after that
        //appear
        //repeat 3 times
    }

    //dodging attacks
    void Dodging()
    {

    }

    //hurt, stunned
    void Stunned()
    {

    }
    #endregion

    //selects either feynman, invisible, mirage or tunnelling
    void SelectNewAction()
    {
        //get a random int
        int randomAction = Random.Range(0, 3);
        //select the action
        switch (randomAction)
        {
            case 0:
                bossState = BossState.Feynmann;
                break;
            case 1:
                bossState = BossState.Invisible;
                break;
            case 2:
                bossState = BossState.Mirage;
                break;
            case 3:
                bossState = BossState.Tunnelling;
                break;
        }
    }

    #region BOSS_STATE_IENUMERATORS
    //stop being idle after idleTime
    IEnumerator IdleIEnum()
    {
        yield return new WaitForSeconds(idleTime);
        //after idle, set to moving
        bossState = BossState.Moving;
    }
    //move for a certain amount of time
    IEnumerator MovingIEnum()
    {
        //get start time
        float startTime = Time.time;
        float timeElapsed = startTime;
        //for movingTime seconds, move
        while(timeElapsed - startTime < movingTime)
        {
            //move around arena centre
            transform.position = arenaCentre + new Vector3(movingRadius * Mathf.Cos((timeElapsed - startTime) * movingSpeed), movingHeight, movingRadius * Mathf.Sin((timeElapsed - startTime) * movingSpeed));
            //increase time
            timeElapsed += Time.deltaTime;
            yield return 0;
        }
        //on finishing, select an action
        SelectNewAction();
    }
    #endregion
}