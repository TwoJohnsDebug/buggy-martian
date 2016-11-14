using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {
    //THISCODEISANANGRYMARTIAN
        public float LevelStartDelay =  2f; // delay to show text screen
        public float turnDelay = .1f; // delay in switching turns to pace better, not instantly move enemies.
        public static GameManager instance; //not assigned as default = null
        public BoardManager boardScript;
        [HideInInspector] public bool playersTurn = true;
        public Player playerscript;

    //THISCOMMENTISANANGRIERMARTIAN
        private Text levelText;
        private GameObject levelImage;
        private List<Enemy> enemies;
        private bool enemiesMoving;
        private bool doingSetup;

        // Use this for initialization
        public void Awake()
        {
            Debug.Log("waking up " + MyGlobals.restarts + " " + MyGlobals.jlevel);
           // if (instance == null)
            instance = this;
         //   else if (instance != this)
            //    {
                //    Destroy(gameObject);
             //       Debug.LogWarning("Destroyed instance, already running", instance);
            //     }
            DontDestroyOnLoad(gameObject); // dont destroy the game object when restart, keeps constant between restarts
            enemies = new List<Enemy>();
            releaseCheck(); //chhecks a constant variable
            boardScript = GetComponent<BoardManager>();
            Debug.Log("logging " + MyGlobals.restarts);
            levelcheck(); // level check
            Debug.Log(MyGlobals.jlevel); // logs the level for debugging purposes, can probably be removed soon
            ClearConsole();
        }
    private void releaseCheck()
    {
        if (MyGlobals.Release == true) // checks a static boolean set in Loader.cs
        {
            #pragma warning disable CS0162 // Unreachable code detected
            MyGlobals.jlevel = 1; // (de)activates level select debug level
            #pragma warning restore CS0162 // Unreachable code detected
        }
       }
    public void ClearConsole() 
    {
       Debug.ClearDeveloperConsole(); // TODO: fix.    currently this is broken, not sure why.
    }
    private void levelcheck()
    {
            Debug.Log("level " + MyGlobals.jlevel + " load " + MyGlobals.restarts);
            InitGame();
       }

      public void InitGame()
        {
             Debug.Log("Doing Setup " + MyGlobals.restarts);
            doingSetup = true; // variable used to stop all other funtions.
            levelImage = GameObject.Find("LevelImage"); // pulls the level image variable from the actual leveltext object
            levelText = GameObject.Find("LevelText").GetComponent<Text>(); // pulls the level text variable from the actual leveltext object
        if (MyGlobals.jlevel == 0)
        {
            levelText.text = "Level Select";
        }
        else
        {
            levelText.text = "Day " + MyGlobals.jlevel;
        }
            levelImage.SetActive(true);
            Invoke("HideLevelImage", LevelStartDelay);
            enemies.Clear();
            boardScript.SetupScene(MyGlobals.jlevel); //tells boardmanager to setup the scene, passing in the level.
        }

        private void HideLevelImage()
        {
        levelImage.SetActive(false);
        doingSetup = false;
        }

        public void GameOver()
        {
            levelText.text = "After " + MyGlobals.jlevel + " days, you starved.";
            levelImage.SetActive(true);
            enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (playersTurn || enemiesMoving || doingSetup)
            return;

            StartCoroutine(MoveEnemies());

        }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay); // yields (does it just surrender?) and waits for the turn delay
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay); // yields and waits for the turn delay
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }
    }
