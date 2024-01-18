using UnityEngine;


public class CarMovement : MonoBehaviour
{
    [SerializeField] private string intersectionLayer;
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private IntersectionMarker currIntersection;
    private Vector3 newDir;
    private float distToTurn;
    private Vector2 turnStart;
    private bool turnPending = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the car is on an intersection, if so, turns to a random neighbouring one
        if (collision.gameObject.tag == "Intersection")
        {
            turnPending = true;
            currIntersection = collision.GetComponent<IntersectionMarker>();
            currIntersection.carsWaiting.Enqueue(this);
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

            // this is absolutely terrible code.
            turnStart = this.transform.position;

            // wait distance then turn car...
            if(Mathf.Abs(angle + 90) < 1)
            {
                distToTurn = 8.1f;
            }

            if (Mathf.Abs(angle - 90) < 1)
            {
                distToTurn = 12.1f;
            }

            //knocks the player back & stuns em for a while when hit
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Intersection")
        {
            currIntersection.carsWaiting.Dequeue();
            currIntersection = null;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().invulnTime <= 0f)
            {
                // add car velocity to delta btween us and car
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0) + (collision.transform.position - this.transform.position).normalized * 15;
                if (!collision.gameObject.GetComponent<PlayerMovement>().stunned)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().StartCoroutine("Stun", 1f);
                }
            }

            if (!GameManager.carsHit.Contains(gameObject))
            {
                GameManager.carsHit.Add(gameObject);
                if (GameManager.carsHit.Count == 3)
                {
                    GameCanvas.addToMessageQueue("HIT 3 CARS! + 10");
                    GameManager.scoreStyle += 10;
                }

                if (GameManager.carsHit.Count == 5)
                {
                    GameCanvas.addToMessageQueue("HIT 5 CARS! + 30");
                    GameManager.scoreStyle += 30;
                }

                if (GameManager.carsHit.Count == 7)
                {
                    GameCanvas.addToMessageQueue("HIT 7 CARS! + 70");
                    GameManager.scoreStyle += 70;
                }
            }

        }
    }

    public void Turn()
    {
        transform.up = newDir;
    }
   
    private void Update()
    {
        // raycast forward and don't hit anyone
        var hit = Physics2D.Raycast(this.transform.position + (2 * this.transform.up), this.transform.up, 1f, LayerMask.GetMask("Car"));
        if(hit)
        {
            // someone is in front of us..
            rb.velocity = Vector2.zero;
            return;
        }

        // frozen because of powerup.
        if(GameManager.timeDelay > 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // waiting on intersection
        if(currIntersection != null && currIntersection.carsWaiting.Peek() != this)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if(Vector2.Distance(turnStart,this.transform.position) > distToTurn && turnPending)
        {
            Turn();
            turnPending = false;
        }


        rb.velocity = transform.up * speed;
    }

}
