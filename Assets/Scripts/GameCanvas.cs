using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{

    public GameObject player;
    public MurderManager murderManager;
    public Text timerText1;
    public Text timerText2;
    public Text scoreText1;
    public Text scoreText2;

    public GameObject cutsceneImage;
    // Start is called before the first frame update



    // this kind of sucks and i'm sorry
    [System.Serializable]
    public class MurderListUI
    {
        public GameObject parent;
        public Text d1;
        public Text d2;
        public Image arrow;
        public Material timerMaterial;
    }

    public MurderListUI[] murderListUI;

    void Start()
    {

    }

    public void playCutscene()
    {
        StartCoroutine("PlayCutscene");
    }

    // Update is called once per frame
    void Update()
    {

        int i = 0;
        foreach (var u in murderListUI)
        {
            // assign murder data to UI
            if (murderManager.activeEvents.Count > i)
            {

                u.parent.SetActive(true);
                var v = murderManager.activeEvents[i];
                var dist = (new Vector2(player.transform.position.x, player.transform.position.y) - v.location).magnitude / 5;
                var dir = Vector2.SignedAngle(Vector2.right, v.location - new Vector2(player.transform.position.x, player.transform.position.y));
                u.d1.text = ((int)dist).ToString();
                u.d2.text = ((int)dist).ToString();
                u.parent.SetActive(true);
                u.arrow.transform.eulerAngles = new Vector3(0, 0, dir);
                u.timerMaterial.SetFloat("_Fullness", Mathf.InverseLerp(0f, v.timeMax, v.timeTillExpire));
            }
            else
            {
                u.parent.SetActive(false);
            }
            ++i;
        }

        timerText1.text = ((int)GameManager.timer).ToString();
        timerText2.text = ((int)GameManager.timer).ToString();

        scoreText1.text = GameManager.totalScore().ToString();
        scoreText2.text = GameManager.totalScore().ToString();
    }

    IEnumerator PlayCutscene()
    {
        while (cutsceneImage.transform.position.x < 2000)
        {
            cutsceneImage.transform.position += new Vector3(2000 * Time.deltaTime, 0, 0);
            yield return null;
        }

        cutsceneImage.transform.position = new Vector3(-1000, 350, 0);
    }
}
