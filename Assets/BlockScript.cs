using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private GameObject ParentContainer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if you collide with an alien
        if (collision.tag == "AlienProjectile")
        {
            // destroy alien Projectile
            Destroy(ParentContainer);

        }
        else if (collision.tag == "PlayerProjectile")
        {
            // destroy alien Projectile
            Destroy(ParentContainer);
        }
        else if(collision.tag == "Alien")
        {
            Destroy(ParentContainer);
            // alien is left unchanged, only block is destroyed
        }
    }
}
