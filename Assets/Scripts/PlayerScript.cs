using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static int playerLives = 3;
    [SerializeField] private GameObject projectilePrefabReference;
    [SerializeField] private GameObject GameCanvas;
    [SerializeField] private TextMeshProUGUI textLivesLeft;

    private float projectileCoolDownTimer = 1.5f;
    
    public static event Action GameOverAction;

    private AudioSource shootAudioSource;
    [SerializeField] private AudioClip shootAudioClip;

    private AudioSource explosionAudioSource;
    [SerializeField] private AudioClip explosionClip;

    // Start is called before the first frame update
    void Start()
    {
        shootAudioSource = gameObject.AddComponent<AudioSource>();
        shootAudioSource.clip = shootAudioClip;
        shootAudioSource.loop = false;
        shootAudioSource.playOnAwake = false;


        explosionAudioSource = gameObject.AddComponent<AudioSource>();
        explosionAudioSource.clip = explosionClip;
        explosionAudioSource.loop = false;
        explosionAudioSource.playOnAwake = false;

    }

    private void OnEnable()
    {
        // subscribe to actions
        GameController.GameStarted += resetPlayer;
        ProjectileScript.ProjectileDestroyedAction += removeProjectileCooldown;
    }

    public void resetPlayer()
    {
        playerLives = 3;
        this.transform.localPosition = new Vector3(0, -325, 0);

        Debug.Log("Player responded to GameStarted Event and reset itself");
    }

    public void removeProjectileCooldown()
    {
        projectileCoolDownTimer = 1.5f; // remove cooldown by simply setting timer = 2.0f
    }

    // Update is called once per frame
    void Update()
    {
        projectileCoolDownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && projectileCoolDownTimer>=1.5f)
        {
            // Fire projectile and reset timer
            fireProjectile();
            projectileCoolDownTimer = 0f;

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(this.transform.localPosition.x > -1000)
            {
                Vector3 newPosition = new Vector3(-250 * Time.deltaTime, 0, 0);
                this.transform.Translate(newPosition);
            }
            

        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (this.transform.localPosition.x < 1000)
            {
                Vector3 newPosition = new Vector3(250 * Time.deltaTime, 0, 0);
                this.transform.Translate(newPosition);

            }

        }
    }

    private void fireProjectile()
    {
        // instantiate prefab, set its parent to player
        GameObject projectileInstance = Instantiate(projectilePrefabReference, new Vector3(0, 0, 0), Quaternion.identity);
        projectileInstance.transform.SetParent(GameCanvas.transform, false);
        projectileInstance.transform.position = this.gameObject.transform.position + new Vector3(0,47,0);

        shootAudioSource.Play();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if you collide with an alien
        if (collision.tag == "Alien")
        {
            playerLives--;
            explosionAudioSource.Play();

            if (playerLives <= 0)
            {
                // Trigger Game Over
                GameOverAction?.Invoke();
            }



        }
        else if (collision.tag == "AlienProjectile")
        {
            playerLives--;
            explosionAudioSource.Play();

            textLivesLeft.text = "LIVES LEFT: " + playerLives.ToString();
            if (playerLives <= 0)
            {
                // Trigger Game Over
                GameOverAction?.Invoke();

            }
        }

    }

    private void OnDisable()
    {
        // subscribe to actions
        GameController.GameStarted -= resetPlayer;
    }
}
