using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurderInstance : MonoBehaviour
{
    //checks if the player "prevented" the murder
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // mureder finished, inform manager.
            FindObjectOfType<MurderManager>().stoppedMurder(this.gameObject);
            
            
            
        }
    }
}
