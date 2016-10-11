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
    private Vector2 touchorigin = Vector2.one;
    [HideInInspector] public int restarts;

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
        vertical = (int)Input.GetAxisRaw("Vertical");
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        horizontal = (int)Input.GetAxisRaw("Horizontal");

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
    #else
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            if (myTouch.phase == TouchPhase.Began)
            {
                touchorigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Ended && touchorigin.x >= 0)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchorigin.x;
                float y = touchEnd.y - touchorigin.y;
                touchorigin.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = x > 0 ? 1 : -1;  
            }
        }
    #endif

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
	}

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);
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
