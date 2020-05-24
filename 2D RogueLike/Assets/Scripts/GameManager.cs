using UnityEngine;
using System.Collections;
using System.Collections.Generic;  //Allows us to use Lists
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public float LevelStartDelay = 2f;
    public float GameOverDelay = 2f;
    public float turnDelay = 0.1f; //Delay between each Player turn

    public static GameManager instance = null; //Static instance of GameManager which allows it to be accessed by any other script.
    public BoardManager boardScript;  //Store a reference to our BoardManager which will set up the level.
     //Current level number, expressed in game as "Day 1".

    public int playerFoodPoints = 200;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText; // will display current level number
    private GameObject levelImage;
    private int level = 1;
    private List<Enemy> enemies; //keep track of enemies and send orders to move
    private bool enemiesMoving;
    private bool doingSetup=true;
    private int startpoint = 0;


    void Awake()
    {
       
        if (instance == null)  //Check if instance already exists
            instance = this;  //if not, set instance to this
        else if (instance != this)     //If instance already exists and it's not this:
            Destroy(gameObject);  //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
     
        if(SceneManager.GetActiveScene().name == "Game")
        {
            DontDestroyOnLoad(gameObject);  //Sets this to not be destroyed when reloading scene
        }
        
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();  //Get a component reference to the attached BoardManager script
        InitGame(); //Call the InitGame function to initialize the first level 
    }

    private void OnLevelWasLoaded(int index) //called everytime a scene is loaded //
    {
        startpoint++;

        if(startpoint>=2){
            level++;
            InitGame();
        }

    }

    void InitGame() //Initializes the game for each level.
    {
        doingSetup = true; //prevent player from moving;

        levelImage = GameObject.Find("Level Image");
        levelText = GameObject.Find("Level Text").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);

        Invoke("HideLevelImage", LevelStartDelay); //delay turning off UI

        enemies.Clear(); //clear enemies from last level and prepare new level
       
        boardScript.SetupScene(level);  //Call the SetupScene function of the BoardManager script, pass it current level number.
        Debug.Log("Current level: " + level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;

    }

    //Have enemies register to gamemanager so gamemanager can issue order
    public void AddEnemyToList(Enemy script) //Add the passed in Enemy to the list of Enemy Objects;
    {
        enemies.Add(script); //Add Enemy to List of enemies;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you DIED!";
        levelImage.SetActive(true);
        enabled = false; //disables game manager

        Invoke("ExitApp",4f);

    }


    void Update()
    {
        if (playersTurn || enemiesMoving ||doingSetup)
            return; //if any are true, we will not execute the following code

        StartCoroutine(MoveEnemies()); //Start moving enemies
    }
    

    IEnumerator MoveEnemies() //Coroutine to move enemies in sequence
    {
        enemiesMoving = true; //while true player unable to move
        yield return new WaitForSeconds(turnDelay); //wait for turndelay
        if (enemies.Count == 0) //if no enemies
        {
            yield return new WaitForSeconds(turnDelay); //this will replace delay caused by enemies moving when there are none
        }

        for (int i = 0; i < enemies.Count; i++) //loop through enemy list
        {
            enemies[i].MoveEnemy(); //Wait for Enemy's movetime before moving next Enemy
            yield return new WaitForSeconds(enemies[i].moveTime); //wait for enemies to move before moving next enemy
        }

        playersTurn = true; //after enemies are finished moving, its player's turn;
        enemiesMoving = false; //Enemies are done moving, sets enemiesMoving to false;
    }

    void ExitApp()
    {
        Debug.Log("Game Exit");
        Application.Quit();

    }

}
