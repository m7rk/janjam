using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAnimation : MonoBehaviour
{
    [SerializeField] float turnFrequency;
    [SerializeField] float turnAmount;
    [SerializeField] Sprite alt;
    [SerializeField] bool randomise;
    void Start()
    {
        if (randomise && Random.Range(0f, 1f) < 0.5f) {
            gameObject.GetComponent<SpriteRenderer>().sprite = alt;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        StartCoroutine(MoveSprite());
    }
private IEnumerator MoveSprite()
    {
        yield return new WaitForSeconds(turnFrequency);
        transform.eulerAngles += Vector3.forward * turnAmount;
        StartCoroutine(MoveSpriteBack());
        yield return null;
    }

    private IEnumerator MoveSpriteBack()
    {
        yield return new WaitForSeconds(turnFrequency);
        transform.eulerAngles -= Vector3.forward * turnAmount;
        StartCoroutine(MoveSprite());
        yield return null;
    }



}
