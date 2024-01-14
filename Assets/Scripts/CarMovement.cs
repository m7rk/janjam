using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private string intersectionLayer;
    [SerializeField] private float speed;

    Vector3 carDirection;
    private Vector3 carTarget;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Intersection")
        {
            carTarget = collision.gameObject.GetComponent<IntersectionMarker>().getRandomDestination();
            carTarget = carTarget - transform.position;
            carTarget = carTarget.normalized;

            transform.eulerAngles = Vector2.SignedAngle(Vector2.up, carTarget) * Vector3.forward;
        } 
    }
   


    private void Update()
    {
        rb.velocity = carTarget * speed;
    }

}
