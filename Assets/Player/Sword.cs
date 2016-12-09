using UnityEngine;
using System.Collections;

public class Sword : Weapon
{
    //sword parameters
    public float timeSlowFactor;

    void Start()
    {
        
    }

    void Update()
    {
        //slows down time
        SlowTime();
    }

    //slow motion
    void SlowTime()
    {
        //if the primary index trigger is pressed
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, holdingController) > 0)
        {
            //slow time by the time factor
            Time.timeScale = 1f / timeSlowFactor;
            Time.fixedDeltaTime = 0.02f / timeSlowFactor;
        }
        //if primary index trigger is not pressed
        else
        {
            //keep normal time
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    //if the sword collides with a bullet, send it back
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "projectile")
        {
            
        }
    }
}