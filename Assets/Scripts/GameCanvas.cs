using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    public Image arrow;
    public Text d1;
    public Text d2;
    public GameObject player;
    public GameObject murder;
    public Text timerText1;
    public Text timerText2;
    public GameManager manager;
    public Text scoreText1;
    public Text scoreText2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       var dist = (player.transform.position - murder.transform.position).magnitude / 5;
       var dir = Vector2.SignedAngle(Vector2.right, murder.transform.position - player.transform.position);
       d1.text = ((int)dist).ToString();
       d2.text = ((int)dist).ToString();
       arrow.transform.eulerAngles = new Vector3(0, 0, dir);

        timerText1.text = ((int)manager.timer).ToString();
        timerText2.text = ((int)manager.timer).ToString();

        scoreText1.text = manager.totalScore().ToString();
        scoreText2.text = manager.totalScore().ToString();
    }
}
