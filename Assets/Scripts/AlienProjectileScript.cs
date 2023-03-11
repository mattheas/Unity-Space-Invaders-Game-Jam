using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienProjectileScript : MonoBehaviour
{
    public static event Action ProjectileCollidedWithPlayer;

    [SerializeField] private GameObject ParentContainer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // projectile should move at constant speed downwards
        this.transform.Translate(new Vector3(0, -290 * Time.deltaTime, 0));

        // projectile destroys itself if it goes off screen
        if (this.transform.localPosition.y < -470)
        {
            Debug.Log("Alien Projectile goes off screen");
            Destroy(ParentContainer);
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if you collide with an alien
        if (collision.tag == "PlayerTag")
        {
            Debug.Log("Alien Projectile collided with Player");
            ProjectileCollidedWithPlayer?.Invoke();

            // destroy alien Projectile
            Destroy(ParentContainer);

        }
        else if (collision.tag == "PlayerProjectile")
        {
            // destroy alien Projectile
            Destroy(ParentContainer);
        }
        else if (collision.tag == "BlockCollider")
        {
            Destroy(ParentContainer);
        }
    }
}


