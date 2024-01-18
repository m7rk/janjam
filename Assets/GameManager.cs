using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static double timer = 120;
    public static int scoreBonus = 0;
    public static int scoreStyle = 0;
    public static int scoreSaves = 0;
    public static float timeDelay = 0;
    public static int highScore = 0;

    // achievements
    public static HashSet<GameObject> carsHit = new HashSet<GameObject>();
    public static HashSet<GameObject> trashesHit = new HashSet<GameObject>();
    public static HashSet<GameObject> manholesFellDown = new HashSet<GameObject>();

    public static void reset()
    {
        timer = 120;
        scoreBonus = 0;
        scoreStyle = 0;
        scoreSaves = 0;
        carsHit.Clear();
        trashesHit.Clear();
        manholesFellDown.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeDelay > 0)
        {
            timeDelay -= Time.deltaTime;
            return;
        }
        else
        {
            timeDelay = 0;
        }

        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SceneManager.LoadScene("Ending");
        }
    }

    public static int totalScore()
    {
        return scoreBonus + scoreStyle + scoreSaves;
    }
}
