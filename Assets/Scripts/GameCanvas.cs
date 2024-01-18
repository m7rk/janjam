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

    public Text infoText;
    private static List<string> messageQueue = new List<string>();
    private float messageTime = 1f;



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
    public Sprite[] cutscenes;

    void Start()
    {
        cutsceneImage.SetActive(true);
    }

    public static void addToMessageQueue(string message)
    {
        messageQueue.Add(message);

    }

    public void playCutscene()
    {
        cutsceneImage.GetComponent<Image>().sprite = cutscenes[Random.Range(0, cutscenes.Length)];
        cutsceneImage.GetComponent<Image>().color = new Color(1, 1, 1,1);
        StartCoroutine("PlayCutscene");
    }

    // Update is called once per frame
    void Update()
    {
        messageTime -= Time.deltaTime;
        if(messageQueue.Count > 0 && messageTime < 0f)
        {
            messageTime -= Time.deltaTime;
            infoText.text = messageQueue[0];
            messageQueue.RemoveAt(0);
            messageTime = 2f;
        }
        if(messageTime > 0f)
        {
            infoText.transform.localScale = new Vector3(Mathf.Min(1,messageTime), Mathf.Min(1, messageTime), Mathf.Min(1, messageTime));
        }
        else
        {
            infoText.transform.localScale = new Vector3(0, 0, 0);
        }

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
        while (cutsceneImage.GetComponent<Image>().color.a > 0)
        {
            cutsceneImage.GetComponent<Image>().color = new Color(1, 1, 1, cutsceneImage.GetComponent<Image>().color.a - 0.01f);
            yield return null;
        }
    }
}
