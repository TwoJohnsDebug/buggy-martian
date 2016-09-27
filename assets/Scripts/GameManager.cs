using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {   
    //THISCODEISANANGRYMARTIAN
        public float LevelStartDelay = 2f;
        public float turnDelay = .1f;
        public static GameManager instance = null;
        public BoardManager boardScript;
        public int playerFoodPoints = 100;
        [HideInInspector] public bool playersTurn = true;

    //THISCOMMENTISANANGRIERMARTIAN
        private Text levelText;
        private GameObject levelImage;
        [HideInInspector] public int level = 1;
        private List<Enemy> enemies;
        private bool enemiesMoving;
        private bool doingSetup;

        // Use this for initialization
        public void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            enemies = new List<Enemy>();
            boardScript = GetComponent<BoardManager>();
            Debug.Log("logging");
            levelcheck();
            Debug.Log(level);
        }
    
    private void levelcheck()
    {
            Debug.Log("level " + level + " load");
            InitGame();
    }

      public void InitGame()
        {
             Debug.Log("Doing Setup");
            doingSetup = true;
            
            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelText.text = "Day " + level;
            levelImage.SetActive(true);
            Invoke("HideLevelImage", LevelStartDelay);
            enemies.Clear();
            boardScript.SetupScene(level);
        }

        private void HideLevelImage()
        {
        levelImage.SetActive(false);
        doingSetup = false;
        }
        
        public void GameOver()
        {
            levelText.text = "After " + level + " days, you starved.";
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
