using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionMarker : MonoBehaviour
{
    private List<Vector3> availablePoints;
    [SerializeField] private LayerMask registeredLayers;

    void Start()
    {
        checkDirections();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    public Vector2 getRandomDestination()
    {
        int rp = Random.Range(1, availablePoints.Count) - 1;

        Vector2 output = new Vector2(availablePoints[rp].x, availablePoints[rp].y);

        return output;
    }

    private void checkDirections()
    {
        availablePoints = new List<Vector3>();

       // Ray2D rUp = Physics2D.Raycast(transform.position + (Vector3.up * 2), Vector2.up, Mathf.Infinity, registeredLayers);
       // Ray2D rRight = Physics2D.Raycast(transform.position + (Vector3.up * 2), Vector2.up, Mathf.Infinity, registeredLayers);
       // Ray2D rDown = Physics2D.Raycast(transform.position + (Vector3.up * 2), Vector2.up, Mathf.Infinity, registeredLayers);
        //RaycastHit2D rLeft = Physics2D.Raycast(transform.position + (Vector3.up * 2), Vector2.up, Mathf.Infinity, registeredLayers);

       // if (rUp != null) 


    }
}
