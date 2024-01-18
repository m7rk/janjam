using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Text saved;
    public Text style;
    public Text bonus;
    public Text total;
    public GameObject teaser;
    private bool setTeaser = false;
    public Text ranking;

    public void Start()
    {
        StartCoroutine("Ranking");
        if(GameManager.highScore < GameManager.totalScore())
        {
            setTeaser = true;
            GameManager.highScore = GameManager.totalScore();
        }
    }
    // Start is called before the first frame update
    public void toTitle()
    {
        GameManager.reset();
        SceneManager.LoadScene("Menu");
    }

    public void again()
    {
        GameManager.reset();
        SceneManager.LoadScene("Main");
    }

    public void Update()
    {
    }

    public void randRanking()
    {
        ranking.text = (new List<string>() { "S", "A", "B", "C", "D" })[Random.Range(0, 5)];
    }

    // round down to nearest 0.
    public string tensPlace(int total)
    {
        return (((int)(total/10))*10).ToString();
    }

    IEnumerator Ranking()
    {
        while (saved.text != tensPlace(GameManager.scoreSaves))
        {
            saved.text = (int.Parse(saved.text) + 5).ToString();
            randRanking();
            yield return null;
        }

        while (saved.text != GameManager.scoreSaves.ToString())
        {
            saved.text = (int.Parse(saved.text) + 1).ToString();
            randRanking();
            yield return null;
        }
        
        //--

        while (style.text != tensPlace(GameManager.scoreStyle))
        {
            style.text = (int.Parse(style.text) + 5).ToString();
            randRanking();
            yield return null;
        }

        while (style.text != GameManager.scoreStyle.ToString())
        {
            style.text = (int.Parse(style.text) + 1).ToString();
            randRanking();
            yield return null;
        }

        //--

        while (bonus.text != tensPlace(GameManager.scoreBonus))
        {
            bonus.text = (int.Parse(bonus.text) + 5).ToString();
            randRanking();
            yield return null;
        }

        while (bonus.text != GameManager.scoreBonus.ToString())
        {
            bonus.text = (int.Parse(bonus.text) + 1).ToString();
            randRanking();
            yield return null;
        }

        //--

        while (total.text != tensPlace(GameManager.totalScore()))
        {
            total.text = (int.Parse(total.text) + 10).ToString();
            randRanking();
            yield return null;
        }

        while (total.text != GameManager.totalScore().ToString())
        {
            total.text = (int.Parse(total.text) + 1).ToString();
            randRanking();
            yield return null;
        }


        if(setTeaser)
        {
            teaser.SetActive(true);
        }
        
        ranking.text = "D";
        if (GameManager.totalScore() > 100)
        {
            ranking.text = "C";
        }
        if (GameManager.totalScore() > 250)
        {
            ranking.text = "B";
        }
        if (GameManager.totalScore() > 450)
        {
            ranking.text = "A";
        }
        if (GameManager.totalScore() > 600)
        {
            ranking.text = "S";
        }
        yield return null;
    }
}
