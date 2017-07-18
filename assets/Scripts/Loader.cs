using UnityEngine;
using System.Collections;
public static class MyGlobals
{
    public static int jlevel; // can change because not const, not assigned a sdefualt is 0
    public static int playerFoodPoints = 100;
}
public class Loader : MonoBehaviour
    {

        public GameObject gameManager;

        // Use this for initialization
        void Awake()
        {
            if (GameManager.instance == null)
                Instantiate(gameManager);
        }
    }