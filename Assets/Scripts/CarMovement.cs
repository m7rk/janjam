using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private string intersectionLayer;

    Vector3 carDirection;
    
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (LayerMask.NameToLayer(intersectionLayer) == collision.gameObject.layer)
       //{

            Debug.Log("It works!!");

       // }
    }


}
