using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float timeToOfset;
    [SerializeField] private float lookAhead;


    public GameObject player;
    readonly float camWallRaycastDistance = 5f; // 7 tiles, minus two for each wall.
    private Vector3 refvector = Vector3.zero;
    private Vector2 directionAffected;


    void Start()
    {
        
    }

    void Update()
    {
        // set the target camera X and Y.
        var targetX = player.transform.position.x;
        var targetY = player.transform.position.y;

        directionAffected = Vector2.one;


        //raycast Y up and down.
        var raycastY = Physics2D.Raycast(player.transform.position, Vector2.up, camWallRaycastDistance, LayerMask.GetMask("Wall"));
        if (raycastY.collider != null)
        {
            // camera intersects with the top wall.
            targetY = raycastY.point.y - camWallRaycastDistance;
            directionAffected.y = 0;
        }

        raycastY = Physics2D.Raycast(player.transform.position, Vector2.down, camWallRaycastDistance, LayerMask.GetMask("Wall"));       
        if (raycastY.collider != null)
        {
            // camera intersects with the bottom wall.
            targetY = raycastY.point.y + camWallRaycastDistance;
            directionAffected.y = 0;
        }

        //raycast X right and left
        var raycastX = Physics2D.Raycast(player.transform.position, Vector2.right, camWallRaycastDistance, LayerMask.GetMask("Wall"));
        if (raycastX.collider != null)
        {
            // camera intersects with the right wall
            targetX = raycastX.point.x - camWallRaycastDistance;
            directionAffected.x = 0;
        }

        raycastX = Physics2D.Raycast(player.transform.position, Vector2.left, camWallRaycastDistance, LayerMask.GetMask("Wall"));
        if (raycastX.collider != null)
        {
            // camera intersects with the left wall
            targetX = raycastX.point.x + camWallRaycastDistance;
            directionAffected.x = 0;
        }


        Vector3 lookaheadVector = new Vector3(directionAffected.x,directionAffected.y, transform.position.z) * PlayerMovement.DirectionInput() * lookAhead ;



        //this.transform.position = new Vector3(targetX, targetY, this.transform.position.z);

        //Using smoothdamp for the smoother camera movement, i saved the old one in case we need a more snappy camera
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetX, targetY, this.transform.position.z), ref refvector, timeToOfset);
    }
}
