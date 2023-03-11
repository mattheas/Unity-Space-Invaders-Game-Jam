using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileScript : MonoBehaviour
{
    public static event Action ProjectileDestroyedAction;

    [SerializeField] private GameObject ParentContainer;

    private AudioSource explosionAudioSource;
    private AudioSource invaderDeathAudioSource;

    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip deathAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        explosionAudioSource = gameObject.AddComponent<AudioSource>();
        explosionAudioSource.clip = explosionClip;
        explosionAudioSource.loop = false;
        explosionAudioSource.playOnAwake = false;

        invaderDeathAudioSource = gameObject.AddComponent<AudioSource>();
        invaderDeathAudioSource.clip = deathAudioClip;
        invaderDeathAudioSource.loop = false;
        invaderDeathAudioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        // projectile should move at constant speed upwards
        this.transform.Translate(new Vector3(0, 300 * Time.deltaTime, 0));

        // projectile destroys itself if it goes off screen
        if (this.transform.localPosition.y > 563f)
        {
            explosionAudioSource.Play();
            Debug.Log("Player Projectile goes off screen");
            //ParentContainer.GetComponent<Image>().enabled = false;
            //ParentContainer.GetComponent<BoxCollider2D>().enabled = false;

            Destroy(ParentContainer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if you collide with an alien
        if (collision.tag == "Alien")
        {
            Debug.Log("Player Projectile collided with Alien");
            // inform player that it has been destroyed to remove the cooldown
            ProjectileDestroyedAction?.Invoke();
            explosionAudioSource.Play();
            ParentContainer.GetComponent<Image>().enabled = false;
            ParentContainer.GetComponent<BoxCollider2D>().enabled = false;

            // destroy Player Projectile
            Destroy(ParentContainer, explosionClip.length);


            // increase score of player
        }
        else if (collision.tag == "AlienProjectile")
        {
            Debug.Log("Player Projectile collided with Alien Projectile");
            // inform player that it has been destroyed to remove the cooldown
            ProjectileDestroyedAction?.Invoke();
            explosionAudioSource.Play();
            invaderDeathAudioSource.Play();
            ParentContainer.GetComponent<Image>().enabled = false;
            ParentContainer.GetComponent<BoxCollider2D>().enabled = false;

            // Player Projectile
            Destroy(ParentContainer, explosionClip.length);

        }
        else if (collision.tag == "BlockCollider")
        {
            ProjectileDestroyedAction?.Invoke();
            explosionAudioSource.Play();
            ParentContainer.GetComponent<Image>().enabled = false;
            ParentContainer.GetComponent<BoxCollider2D>().enabled = false;

            Destroy(ParentContainer, explosionClip.length);
        }

    }

}
