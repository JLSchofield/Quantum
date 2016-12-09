using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float movementSpeed;

    void Update()
    {
        Move();
    }

    //move the bullet forward
    void Move()
    {
        transform.position += transform.forward.normalized * movementSpeed * Time.deltaTime;
    }

    //if the bullet collides with the boss
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "boss")
        {
            //take damage
            collider.GetComponent<Quantum>().bossState = Quantum.BossState.Stunned;
            //deactivate the bullet
            gameObject.SetActive(false);
        }
    }
}