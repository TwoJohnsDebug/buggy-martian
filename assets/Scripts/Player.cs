using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;
    private Animator animator;
    private int food;
    [HideInInspector] public int restarts = 0;

    // Use this for initialization
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = MyGlobals.playerFoodPoints;
        foodText.text = "Food: " + food;

        base.Start();
    }

	// Update is called once per frame
	void Update ()
    {
        if (GameManager.instance.playersTurn == false)
            return;

        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical); 
	}
     
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        //RaycastHit2D hit;
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Debug.Log("on exit " + restarts);
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + "     Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + "     Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag=="Level")
        {
            MyGlobals.jlevel++;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall (wallDamage);
        animator.SetTrigger ("playerChop");
    }

    private void Restart()
    {
        //Application.LoadLevel(Application.loadedLevel);
        Debug.Log("Restarting... " + restarts);
        restarts++;
        Debug.Log("Restarts: " + restarts);
        MyGlobals.jlevel++;
        MyGlobals.playerFoodPoints = food;
        Debug.Log(MyGlobals.jlevel + " " + MyGlobals.playerFoodPoints);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Debug.Log("Restarts after reset: " + restarts);
    }
     public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }
    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
