using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    readonly float camWallRaycastDistance = 5f; // 7 tiles, minus two for each wall.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // set the target camera X and Y.
        var targetX = player.transform.position.x;
        var targetY = player.transform.position.y;

        //raycast Y up and down.
        var raycastY = Physics2D.Raycast(player.transform.position, Vector2.up, camWallRaycastDistance, LayerMask.GetMask("Wall"));
        if (raycastY.collider != null)
        {
            // camera intersects with the top wall.
            targetY = raycastY.point.y - camWallRaycastDistance;
        }

        raycastY = Physics2D.Raycast(player.transform.position, Vector2.down, camWallRaycastDistance, LayerMask.GetMask("Wall"));       
        if (raycastY.collider != null)
        {
            // camera intersects with the bottom wall.
            targetY = raycastY.point.y + camWallRaycastDistance;
        }

        this.transform.position = new Vector3(targetX, targetY, this.transform.position.z);
    }
}
