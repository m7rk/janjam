using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionMarker : MonoBehaviour
{
    //this object just marks one of the points where a car can travel to/from

    public List<Vector3> availablePoints;
    

    void Start()
    {
        checkDirections();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    //this function is called by the cars to see where can they travel after reaching this node
    public Vector2 getRandomDestination()
    {
        int rp = Random.Range(0, availablePoints.Count) ;

        Vector2 output = new Vector2(availablePoints[rp].x, availablePoints[rp].y);

        return output;
    }

    //checks for all neighbouring nodes in a + shape from this one
    private void checkDirections()
    {
        LayerMask registeredLayers = LayerMask.GetMask("Intersection", "Wall");
        availablePoints = new List<Vector3>();

        Physics2D.queriesHitTriggers = true;

        RaycastHit2D rUp = Physics2D.Raycast(transform.position + (Vector3.up * 11), Vector2.up, Mathf.Infinity, registeredLayers);
        if (rUp.collider != null && rUp.collider.gameObject.tag != "Wall") { availablePoints.Add(rUp.point); }

        RaycastHit2D rRight = Physics2D.Raycast(transform.position + (Vector3.right * 11), Vector2.right, Mathf.Infinity, registeredLayers);
        if (rRight.collider != null && rRight.collider.gameObject.tag != "Wall") { availablePoints.Add(rRight.point); }

        RaycastHit2D rDown = Physics2D.Raycast(transform.position + (Vector3.down * 11), Vector2.down, Mathf.Infinity, registeredLayers);
        if (rDown.collider != null && rDown.collider.gameObject.tag != "Wall") { availablePoints.Add(rDown.point); }

        RaycastHit2D rLeft = Physics2D.Raycast(transform.position + (Vector3.left * 11), Vector2.left, Mathf.Infinity, registeredLayers);
        if (rLeft.collider != null && rLeft.collider.gameObject.tag != "Wall") { availablePoints.Add(rLeft.point); }




    }
}
