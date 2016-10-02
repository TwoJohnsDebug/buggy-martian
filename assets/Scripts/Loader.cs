using UnityEngine;
using System.Collections;
public static class MyGlobals
{
    // public const string Prefix = "ID_"; // cannot change
    public static int jlevel = 0; // can change because not const
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