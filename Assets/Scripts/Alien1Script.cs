using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alien1Script : MonoBehaviour
{
    [SerializeField] GameObject Alien1Container;

    public static event Action SwitchRowDirectionAction;
    public static event Action AlienDestroyedByProjectileAction;

    private int alienRow;
    private int alienColumn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAlienRow(int value)
    {
        alienRow = value;
    }

    public void setAlienColumn (int value)
    {
        alienColumn = value;
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if you collide with an alien
        if (collision.tag == "PlayerTag")
        {
            // destroy Alien and Player Projectile
            //Destroy(Alien1Container);
            Alien1Container.GetComponent<Image>().enabled = false;
            Alien1Container.GetComponent<BoxCollider2D>().enabled = false;

        }
        else if (collision.tag == "PlayerProjectile")
        {
            // destroy both Alien and Player Projectile
            //Destroy(Alien1Container);
            Alien1Container.GetComponent<Image>().enabled = false;
            Alien1Container.GetComponent<BoxCollider2D>().enabled = false;

            AlienDestroyedByProjectileAction?.Invoke();

            // note down which alien was "destroyed"
            GameController.alienIndexToFireProjectile[alienColumn] = alienRow-1;

            Debug.Log("Alien projectile could fire from row and column :" + GameController.alienIndexToFireProjectile[alienColumn].ToString() + ", " + alienColumn.ToString());


        }
        else if (collision.tag == "CanvasEdgeCollider")
        {
            Debug.Log("Alien has collided with edge of screen");
            SwitchRowDirectionAction?.Invoke();
        }

    }
}
