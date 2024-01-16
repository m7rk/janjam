using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public double timer = 60;

    int scoreBonus = 0;
    int scoreStyle = 0;
    int scoreSaves = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SceneManager.LoadScene("Ending");
        }
    }

    public int totalScore()
    {
        return scoreBonus + scoreStyle + scoreSaves;
    }
}
