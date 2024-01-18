using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManholeTrigger : MonoBehaviour
{
    [SerializeField] private float fallTime;
    private float timer = 15f;
    private Vector3 runningFrom;
    private Transform playerObject;
    private SpriteRenderer sr;
    private bool isRespawned = true;
    private Rigidbody2D rb;

    [SerializeField] private Sprite closedSprite;
    [SerializeField] private GameObject lid;
    [SerializeField] private SpriteRenderer srChild;
    [SerializeField][Range(0f,1f)] float ChanceToBeClosed;



    private void Awake()
    {
        if (Random.Range(0f, 1f) < ChanceToBeClosed) CloseManhole();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb = collision.attachedRigidbody;
            playerObject = collision.gameObject.transform;
            runningFrom = rb.velocity.normalized;
            rb.simulated = false;
            sr = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
            collision.gameObject.transform.position = transform.position;
            playerObject.gameObject.GetComponent<PlayerMovement>().enabled = false;

            isRespawned = false;
            timer = 0;

            if (!GameManager.manholesFellDown.Contains(this.gameObject))
            {
                GameManager.manholesFellDown.Add(this.gameObject);
                if (GameManager.manholesFellDown.Count == 1)
                {
                    GameCanvas.addToMessageQueue("FELL DOWN A MANHOLE! + 5");
                    GameManager.scoreStyle += 5;
                }
                if (GameManager.manholesFellDown.Count == 3)
                {
                    GameCanvas.addToMessageQueue("FELL DOWN 3 MANHOLES! + 15");
                    GameManager.scoreStyle += 15;
                }
                if (GameManager.manholesFellDown.Count == 5)
                {
                    GameCanvas.addToMessageQueue("FELL DOWN 3 MANHOLES! + 30");
                    GameManager.scoreStyle += 30;
                }
            }
        }
    }

    private void Update()
    {
        if (timer < fallTime)
        {
            float progress = Mathf.InverseLerp(0, fallTime / 1.5f, timer);
            sr.color = new Color(1, 1, 1, 1 - progress);
            playerObject.localScale = ((1 - progress) * new Vector3(1, 1, 0)) + Vector3.forward;
            playerObject.eulerAngles += Vector3.forward * 5;

            timer += Time.deltaTime;
        }
        else if (!isRespawned) respawnPlayer();
    }

    void respawnPlayer()
    {
        playerObject.localScale = Vector3.one;
        playerObject.position = transform.position - (runningFrom) * 1.5f;
        rb.simulated = true;
        rb.velocity = Vector3.zero;
        playerObject.gameObject.GetComponent<PlayerMovement>().enabled = true;

        sr.color = new Color(1, 1, 1, 1);
        isRespawned = true;
        PlayerMovement.PlayerParticle.Play();
    }

    void CloseManhole()
    {
        srChild.sprite = closedSprite;
        srChild.color = Color.white;
        Destroy(lid);
        Destroy(this);
        


    }
}
