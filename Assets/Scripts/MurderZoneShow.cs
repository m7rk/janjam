using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurderZoneShow : MonoBehaviour
{
    //this thing is merely a mark for the zone where a murder can happen
    //it gives the murder object places to potentially spawn on

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    private void Awake()
    {

          GameObject murderSpotHolder = GameObject.Find("!Murder!");
           murderSpotHolder.GetComponent<Murder>().murderSpotList.Add(transform.position);
    }
}
