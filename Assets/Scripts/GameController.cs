using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int highestScore = 0;
    private int sessionScore = 0;

    [SerializeField] private Button PlayButton;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject GameCanvas;
    [SerializeField] private TextMeshProUGUI textHighScore;
    [SerializeField] private TextMeshProUGUI textCurrentScore;
    [SerializeField] private TextMeshProUGUI textLivesLeft;
    [SerializeField] private TextMeshProUGUI textLastScore;

    public static event Action GameStarted;
    public bool gameStarted = false; 

    [SerializeField] private GameObject AlienGridContainer; // this has the vertical layout group
    [SerializeField] private GameObject[] AlienPrefabs = new GameObject[5];
    [SerializeField] private GameObject[] AlienRows = new GameObject[5]; 
    [SerializeField] private GameObject AlienBulletPrefab;

    // index from 0 to 11 represents a column, the value stored in the array 0 to 4 represents which alien shall be firing
    [SerializeField] public static int[] alienIndexToFireProjectile = new int[11];

    [SerializeField] private bool moveRowsRight = true;
    private float moveRowTimer = 0f;
    private float alienFireProjectileTimer = 0f;


    [SerializeField] private bool rowHitsEdge = false;

    [SerializeField] private bool waveBeat = false;


    private void OnEnable()
    {
        Alien1Script.SwitchRowDirectionAction += switchRowDirection;
        Alien1Script.AlienDestroyedByProjectileAction += increaseSessionScore;
        PlayerScript.GameOverAction += gameOverMethod;
    }


    // Start is called before the first frame update
    void Start()
    {
        int waveNumber = PlayerPrefs.GetInt("wavesBeaten", 0);

        // just opening game
        if (waveNumber == 0)
        {
            MainMenuCanvas.SetActive(true);
            GameCanvas.SetActive(false);

            sessionScore = 0;
            textCurrentScore.text = "SCORE: " + sessionScore.ToString();

            textLivesLeft.text = "LIVES LEFT: 3";

            // set highscore
            highestScore = PlayerPrefs.GetInt("highestScore", 0);
            textHighScore.text = "HIGHSCORE: " + highestScore.ToString();

            for (int i = 0; i < 11; i++)
            {
                alienIndexToFireProjectile[i] = 4;
            }
        } else
        {
            // beaten a wave so go straight back into game 
            MainMenuCanvas.SetActive(false);
            GameCanvas.SetActive(true);

            PlayerPrefs.GetInt("scoreSoFar", sessionScore);
            textCurrentScore.text = "SCORE: " + sessionScore.ToString();

            textLivesLeft.text = "LIVES LEFT: 3";

            for (int i = 0; i < 11; i++)
            {
                alienIndexToFireProjectile[i] = 4;
            }

            PlayButtonClicked();
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            moveRowTimer += Time.deltaTime;
            alienFireProjectileTimer += Time.deltaTime;

            if (moveRowTimer > 0.75f)
            {
                if(rowHitsEdge)
                {
/*                    if (moveRowsRight)
                    {
                        AlienRows[0].transform.Translate(new Vector3(60, 0, 0));
                        AlienRows[1].transform.Translate(new Vector3(60, 0, 0));
                        AlienRows[2].transform.Translate(new Vector3(60, 0, 0));
                        AlienRows[3].transform.Translate(new Vector3(60, 0, 0));
                        AlienRows[4].transform.Translate(new Vector3(60, 0, 0));

                    }
                    else
                    {
                        AlienRows[0].transform.Translate(new Vector3(-60, 0, 0));
                        AlienRows[1].transform.Translate(new Vector3(-60, 0, 0));
                        AlienRows[2].transform.Translate(new Vector3(-60, 0, 0));
                        AlienRows[3].transform.Translate(new Vector3(-60, 0, 0));
                        AlienRows[4].transform.Translate(new Vector3(-60, 0, 0));

                    }*/

                    AlienRows[0].transform.Translate(new Vector3(0, -10, 0));
                    AlienRows[1].transform.Translate(new Vector3(0, -10, 0));
                    AlienRows[2].transform.Translate(new Vector3(0, -10, 0));
                    AlienRows[3].transform.Translate(new Vector3(0, -10, 0));
                    AlienRows[4].transform.Translate(new Vector3(0, -10, 0));
                    rowHitsEdge = false;
                    moveRowsRight = !moveRowsRight;

                }



                if (moveRowsRight)
                {
                    AlienRows[0].transform.Translate(new Vector3(60, 0, 0));
                    AlienRows[1].transform.Translate(new Vector3(60, 0, 0));
                    AlienRows[2].transform.Translate(new Vector3(60, 0, 0));
                    AlienRows[3].transform.Translate(new Vector3(60, 0, 0));
                    AlienRows[4].transform.Translate(new Vector3(60, 0, 0));

                }
                else
                {
                    AlienRows[0].transform.Translate(new Vector3(-60, 0, 0));
                    AlienRows[1].transform.Translate(new Vector3(-60, 0, 0));
                    AlienRows[2].transform.Translate(new Vector3(-60, 0, 0));
                    AlienRows[3].transform.Translate(new Vector3(-60, 0, 0));
                    AlienRows[4].transform.Translate(new Vector3(-60, 0, 0));

                }
                     
                

/*                // decrement row index
                if (rowIndex == 0)
                {
                    rowIndex = 4;                    
                }
                else
                {
                    rowIndex--;
                }*/


                //reset the timer
                moveRowTimer = 0f;
            }


            if (alienFireProjectileTimer > 1f)
            {
                waveBeat = true;
                // check game over condition
                for(int i = 0; i<11; i++)
                {
                    if(alienIndexToFireProjectile[i] != -1)
                    {
                        waveBeat = false;
                    }
                }

                if (!waveBeat)
                {               
                    // fire projectile randomlyf
                    int alienColumnIndex = UnityEngine.Random.Range(0, 11);
                    int alienRowIndex = alienIndexToFireProjectile[alienColumnIndex];

                    // re-roll where the alien projectile fires from if all the aliens in that column have been destroyed
                    while(alienRowIndex == -1)
                    {
                        alienColumnIndex = UnityEngine.Random.Range(0, 11);
                        alienRowIndex = alienIndexToFireProjectile[alienColumnIndex];

                    }

                    GameObject alienProjectile = Instantiate(AlienBulletPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    alienProjectile.transform.SetParent(AlienRows[alienRowIndex].transform.GetChild(alienColumnIndex).gameObject.transform, false);
                    alienProjectile.transform.localPosition = new Vector3(0, -20, 0);

                    alienFireProjectileTimer = 0f;

                }

            }


            if (waveBeat)
            {
                // reset level with faster moving enemies
                // do this by switching scenes, then coming back to sample scene and everyuthing should be reset 
                int waveNumber = PlayerPrefs.GetInt("wavesBeaten", 0);
                PlayerPrefs.SetInt("wavesBeaten", waveNumber + 1);
                PlayerPrefs.SetInt("scoreSoFar", sessionScore);


                PlayerPrefs.Save();
                SceneManager.LoadScene(1);

            }


        }
    }

    public void switchRowDirection()
    {
        if (!rowHitsEdge)
        {
            //StartCoroutine(WaitCoroutine());
            rowHitsEdge = true;
        }

    }

    private void increaseSessionScore()
    {
        sessionScore += 20;
        textCurrentScore.text = "SCORE: " + sessionScore.ToString();
    }

/*    IEnumerator WaitCoroutine()
    {
        //yield on a new YieldInstruction that waits for 0.3f seconds.
        yield return new WaitForSeconds(0.3f);

        //nextIterationMoveRowsRight = !moveRowsRight;
        rowHitsEdge = true;
        rowHitIndex = rowIndex + 1; // very sketchy, this depends on the if statement in update that changes the value of rowIndex to run first 
        Debug.Log("Set rowHitsEdge to true and record the row hit index to be:  " + rowHitIndex.ToString());

    }*/

    public void PlayButtonClicked()
    {
        // disable main menu canvas
        MainMenuCanvas.SetActive(false);
        GameCanvas.SetActive(true);

        // player listens for this
        GameStarted?.Invoke();
        gameStarted = true;

        // instantiate all the aliens in the grid
        for (int i=0; i<5; i++)
        {
            for(int j=0; j<11; j++)
            {
                // instantiate prefab, set its parent to container
                GameObject AlienInstance = Instantiate(AlienPrefabs[i], new Vector3(0, 0, 0), Quaternion.identity);
                AlienInstance.transform.SetParent(AlienRows[i].gameObject.transform, false);

                AlienInstance.GetComponent<Alien1Script>().setAlienColumn(j);
                AlienInstance.GetComponent<Alien1Script>().setAlienRow(i);
            }

        }

    }

    public void gameOverMethod()
    {
        // retrieve high score from PlayerPrefs
        highestScore = PlayerPrefs.GetInt("highestScore", 0);

        // check if high score was beaten
        if (sessionScore > highestScore)
        {
            PlayerPrefs.SetInt("highestScore", sessionScore);
            PlayerPrefs.Save();
        }

        // disable Game Canvas and enable main menu canvas 
        MainMenuCanvas.SetActive(true);
        GameCanvas.SetActive(false);

        // set last score
        textLastScore.text = "LAST SCORE: " + sessionScore.ToString();

        // set highscore
        highestScore = PlayerPrefs.GetInt("highestScore", 0);
        textHighScore.text = "HIGHSCORE: " + highestScore.ToString();
    }

    private void OnDisable()
    {
        Alien1Script.SwitchRowDirectionAction -= switchRowDirection;
    }
}
