using UnityEngine;
using System.Collections;

public class Gun : Weapon
{

    Player player;

    //gun and bullet variables
    public GameObject gunGO;
    public GameObject bulletGO;
    public int magazineSize;
    GameObject[] bullets;

    //rapidfire parameters
    public int roundsPerSecond;
    float timeBetweenRounds;
    float timeSinceLastRound;

    void Start()
    {
        //get the player object
        player = GetComponent<Player>();

        //fill bullet pool
        bullets = new GameObject[magazineSize];
        for (int i = 0; i < magazineSize; i++)
        {
            //instantiate new bullet
            GameObject newBullet = Instantiate(bulletGO);
            //deactivate the bullet
            newBullet.SetActive(false);
            //add it to the array
            bullets[i] = newBullet;
        }
        //set bullet time variables
        timeBetweenRounds = 1f / (float)roundsPerSecond;
        timeSinceLastRound = 0f;
    }

    void Update()
    {
        //increase time
        timeSinceLastRound += Time.deltaTime;
        
        //fires the gun
        Fire();
    }

    //fire a bullet
    void Fire()
    {
        //check if the right trigger is held down
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, holdingController) > 0)
        {
            //if enough time has elapsed
            if (timeSinceLastRound > timeBetweenRounds)
            {
                //get n inactive bullet
                foreach (GameObject bullet in bullets)
                {
                    //if the currently select bullet is not active, use it
                    if (!bullet.activeSelf)
                    {
                        //set time since fired
                        timeSinceLastRound = 0f;
                        //set the bullet's initial position to be the same as the gun's
                        bullet.transform.position = gunGO.transform.position;
                        //set the bullet's initial rotation to be the same as the gun's
                        bullet.transform.rotation = gunGO.transform.rotation;
                        //activate button game object
                        bullet.SetActive(true);
                    }
                    //if the bullet is active, move onto the next in the bullets array
                    else continue;
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "projecticle")
        {
            Destroy(collider.gameObject);
        }
    }
}