using UnityEngine;
using System.Collections;

//base class for each weapon
//sets how objects can be picked up/thrown
public class Weapon : MonoBehaviour
{
    //rigidbody component of the weapon
    Rigidbody rigidbodyComponent;

    //minimum distance required for picking up
    float maxPickUpDist = 0.25f;

    //the touch controller that the player is holding the weapon with
    public OVRInput.Controller holdingController;


    void Start()
    {
        //initially set holding controller
        holdingController = OVRInput.Controller.None;
        //get rigidbody component
        rigidbodyComponent = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        //check if the player is holding a weapon
        Hold();

        //if the hand trigger on the holding controller is released
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, holdingController) == 0f && holdingController != OVRInput.Controller.None)
        {
            LetGo();
        }

    }

    //player grabs the weapon
    void Hold()
    {
        //check the left and right controllers for input
        GetController(OVRInput.Controller.LTouch);
        GetController(OVRInput.Controller.RTouch);
        //if the player is holding the weapon with one of the controllers
        if(holdingController != OVRInput.Controller.None)
        {
            //set position and rotation of the weapon
            transform.position = OVRInput.GetLocalControllerPosition(holdingController);
            transform.rotation = OVRInput.GetLocalControllerRotation(holdingController);
            rigidbodyComponent.isKinematic = false;
        }

    }

    //player lets go of the weapon
    void LetGo()
    {
        //set rigidbody to be kinematic
        rigidbodyComponent.isKinematic = true;
        //add force equal to the velocity of the touch controller of the frame when the weapon was released
        rigidbodyComponent.AddForce(OVRInput.GetLocalControllerVelocity(holdingController), ForceMode.VelocityChange);
        //let go of the weapon
        holdingController = OVRInput.Controller.None;
    }

    //sets which controller the player is holding the weapon with
    OVRInput.Controller GetController(OVRInput.Controller controller)
    {
        //if the primary trigger on the controller is held down and the distance between the controller and weapon is less than maxPickUpDist
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller) > 0f && Vector3.Distance(gameObject.transform.position, OVRInput.GetLocalControllerPosition(controller)) < maxPickUpDist)
        {
            //set the holding controller
            return controller;
        }else
        {
            return OVRInput.Controller.None;
        }
    }

}