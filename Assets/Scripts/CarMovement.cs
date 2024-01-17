using UnityEngine;


public class CarMovement : MonoBehaviour
{
    [SerializeField] private string intersectionLayer;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Vector3 newDir;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the car is on an intersection, if so, turns to a random neighbouring one
        if (collision.gameObject.tag == "Intersection")
        {
            Vector3 carTarget;
            newDir = -transform.up;
            while (transform.up == -newDir)
            {
                //reroll destination until we get a new direction. Don't go backwards.
                carTarget = collision.gameObject.GetComponent<IntersectionMarker>().getRandomDestination();
                newDir = (carTarget - collision.gameObject.transform.position).normalized;
            }
            // if we're going forward, don't do anything.
            // if we're going left, go left after 3 units.
            // if we're going right, go right after 6 units.
            var angle = (Vector2.SignedAngle(transform.up, newDir));

            // check if angle is close to zero.
            if(Mathf.Abs(angle + 90) < 1)
            {
                // we're going left, turn left after 3 units.
                Invoke("Turn", 0.8f);
            }

            if (Mathf.Abs(angle - 90) < 1)
            {
                // we're going left, turn left after 3 units.
                Invoke("Turn", 1.3f);
            }

            //knocks the player back & stuns em for a while when hit
        } else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = rb.velocity.normalized * 100;
            collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine("Stun", 2f);
        }
    }

    public void Turn()
    {
        transform.up = newDir;
    }
   
    private void Update()
    {
        rb.velocity = transform.up * speed;
    }

}
