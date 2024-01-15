using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murder : MonoBehaviour
{
    [SerializeField] private float timeTillExpire;
    [SerializeField] private Material timerMaterial;
    [SerializeField] public List<Vector2> murderSpotList;
    [SerializeField] private int murdersLeftToWin;

    static public int savedPeople;
    static public int lostPeople;

    private float expireTimer;

    //this script gouverns how the murder object itself behaves
   


    void Start()
    {
        PickSpot();
    }


    void Update()
    {
        //this gouverns how the timer for the murder works
        if (expireTimer >= 0)
        {
            expireTimer -= Time.deltaTime;
            timerMaterial.SetFloat("_Fullness", Mathf.InverseLerp(0f, timeTillExpire, expireTimer));

        }
        else 
        { 
            PickSpot();
            lostPeople++;
        }


    }

    //checks if the player "prevented" the murder
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag == "Player")
            {
                  PickSpot();
            savedPeople++;
            }
    }

    //initiates a murder on a random spot out of the list, then removes the spot from the list
    private void PickSpot()
    {
        if (murderSpotList.Count <= murdersLeftToWin) Win();
        else
        {
            int rng = Random.Range(0, murderSpotList.Count);

            transform.position = murderSpotList[rng];
            murderSpotList.RemoveAt(rng);

            expireTimer = timeTillExpire;
        }

    }
    //Function is called when the game is won
    private void Win()
    {
        //idk what to do here 
        Debug.Log("Lost: " + lostPeople + " Saved: " + savedPeople);
        Destroy(gameObject);
    }


}
