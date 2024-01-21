using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transitioner : MonoBehaviour
{
    private string nextScene = null;
    private float nextSceneTimer;
    public AudioMixer mixer;
    public Image hider;
    private RectTransform rt;

    public void StartTransitionTo(string nextScene)
    {
        rt.anchoredPosition = new Vector2(-1200, 1200);
        this.nextScene = nextScene;
        nextSceneTimer = 1.0f;
    }

    public void Start()
    {
        rt = hider.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x + 1300 * Time.deltaTime, rt.anchoredPosition.y + -1300 * Time.deltaTime);
        if (nextScene != null)
        {

            mixer.SetFloat("Volume", (nextSceneTimer - 1f) * 80);
            mixer.SetFloat("Pitch", nextSceneTimer);
            nextSceneTimer -= Time.fixedDeltaTime;
            if (nextSceneTimer <= 0.0f)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {

        }
    }
}
