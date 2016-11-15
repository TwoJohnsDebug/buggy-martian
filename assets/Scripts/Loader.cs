using UnityEngine;
using System.Collections;
public static class MyGlobals
{
    public static int jlevel; // can change because not const, not assigned a sdefualt is 0
    public static int playerFoodPoints = 100; // food points, global as constant between restarts.
    public const bool Release = false; // constant, do not change.
    public static int restarts = 0; // restarts global
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