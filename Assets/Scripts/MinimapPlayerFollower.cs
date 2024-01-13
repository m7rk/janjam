using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPlayerFollower : MonoBehaviour
{
    public Transform followThis;

    void Update()
    {
        transform.position = new Vector3(followThis.position.x, followThis.position.y, transform.position.z);
    }
}
