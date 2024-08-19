using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool moving;
    public int size;
    public bool dead;
    [SerializeField] float moveDuration = 0.5f;
    [SerializeField] float castRange = 300;
    [SerializeField] LayerMask wallMask;
    [SerializeField] LayerMask tokenMask;

    private Vector2 originTransform;
    public float timer;
    RaycastHit2D target;
    private Vector2 targetPosition;


    private Collider2D lastPushTile;
    private Animator animator;
    private ParticleSystem deathParticles;
    public float CameraShakeTime;
    public float MaxCameraTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dead = true;
        deathParticles = GetComponentInChildren<ParticleSystem>();
        deathParticles.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Set Animator variables each frame
        animator.SetBool("moving", moving);
        animator.SetInteger("size", size);
        animator.SetBool("dead", dead);

        // If the player is already moving, continue moving
        if (moving)
        {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(originTransform, targetPosition, timer / moveDuration);
        }

        // If the player is not moving and has input on any axis, move.
        else if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !dead)
        {
            Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    private void LateUpdate()
    {
        // If the player has reached their destination, stop moving
        if (moving && transform.position == toVector3(targetPosition)) 
        {
            RecenterPosition();
            timer = 0;

            if (target.collider.CompareTag("Kill")) { Die(); }
            else if (target.collider.CompareTag("Finish")) { target.collider.GetComponent<Goal>().GoalCheck(size); }

            moving = false;
            transform.eulerAngles = Vector3.zero;
            
            // If the currently registered target is a pushtile, force the player to move
            if (target.collider.CompareTag("PushTile"))
            {
                Move(lastPushTile.transform.right.x, lastPushTile.transform.right.y);
            }
        }
    }

    // Snap the player's position to the grid. X and Y position should always end in .5
    void RecenterPosition()
    {
        transform.position = new Vector2(HalfRound(transform.position.x), HalfRound(transform.position.y));
    }

    // Fire a circlecast in the direction of the input until it hits a wall. Then, move there.
    void Move(float hInput, float vInput)
    {
        moving = true; // Declare the player is moving
        Vector2 pointTo;


        if (-0.01 >= hInput || 0.01 <= hInput) { pointTo = new Vector2(hInput, 0).normalized; } // If there is horizontal input, fire the raycast left or right
        else if (-0.01 >= vInput || 0.01 <= vInput) { pointTo = new Vector2(0, vInput).normalized; } // If there is no horizontal input, but there is vertical input, fire raycast up or down
        else { Debug.LogError("Tried to move with no direction!"); return; } // If there is somehow no input, throw an error and return void.


        RaycastHit2D[] allHits = Physics2D.CircleCastAll(transform.position, 0.35f, pointTo, castRange, wallMask); // Fire the circlecast to detect all objects in its path
        foreach (RaycastHit2D hit in allHits) 
        {
            // If the target is a OneWay wall, check if the player is on the correct side of the wall
            // Check this by seeing if the side of the tile with the collider is farther away than the tile's center. If it is, disable the collider and redo the circlecast
            if (hit.collider.CompareTag("OneWay") && Vector3.Distance(transform.position, toVector3(hit.point)) > Vector3.Distance(transform.position, hit.transform.position))
            { // Do nothing and continue the loop
            }
                
            // Once the target is determined to be a valid target, mark it and break the loop
            else
            {
                target = hit;

                if (target.collider.CompareTag("Finish")) { targetPosition = target.collider.transform.position; }
                else { targetPosition = target.point; }
                break;
            }
        }

        // Once a target is determined, cast for all tokens between the player and the target
        RaycastHit2D[] tokenHits = Physics2D.CircleCastAll(transform.position, 0.35f, pointTo, Vector3.Distance(transform.position, toVector3(targetPosition)), tokenMask);
        
        // for each token detected, determine how much time until the player reaches the token, then trigger the collection routine
        foreach (RaycastHit2D token in tokenHits)
        {
            token.collider.GetComponent<Coin>().resizeWhenTimer = (Vector3.Distance(transform.position, token.transform.position) / Vector3.Distance(transform.position, target.transform.position)) * moveDuration;
            token.collider.GetComponent<Coin>().StartCoroutine("Collect");
        }

        try { lastPushTile.enabled = true; } catch { } // Renable the collider of the last push tile contacted.
        
        
        // If the target is a push tile or a goal tile, disable the tile's collider. This prevents the player from getting stuck on it.
        if (target.collider.CompareTag("PushTile") || target.collider.CompareTag("Finish"))
        {
            lastPushTile = target.collider;
            lastPushTile.enabled = false;
        }
        
        // If the raycast hit a wall next to the player, return void
        else if (
            target.point.x < transform.position.x + 1 && target.point.x > transform.position.x - 1 &&
            target.point.y < transform.position.y + 1 && target.point.y > transform.position.y - 1 )
        { moving = false; return; }
        
        originTransform = transform.position; // log the player's starting position as it moves

        // Rotate the player to face the direction of motion
        if (pointTo.y != 0) { transform.eulerAngles = new Vector3(0, 0, Mathf.Asin(pointTo.y) * Mathf.Rad2Deg - 90); }
        else if (pointTo.x != 0) { transform.eulerAngles = new Vector3(0, 0, Mathf.Acos(pointTo.x) * Mathf.Rad2Deg - 90); }
        else { Debug.LogError("ERROR: Invalid target for rotation. Vector: (" + pointTo.x + ", " + pointTo.y + ")"); }
        
    }

    public void Die()
    {
        dead = true;
        moving = false;
        deathParticles.gameObject.SetActive(true);
    }
    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReEnableMovement() { dead = false; }
    // Utility functions
    static float HalfRound(float num) => (Mathf.FloorToInt(num) * 2 + 1) / 2f; // Round num to the nearest float ending in .5
    static Vector3 toVector3(Vector2 vector) => new Vector3(vector.x, vector.y); // Converts a vector 2 to Vector 3

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target.point, 0.35f);
        Gizmos.DrawLine(transform.position, target.point);
    }

    IEnumerator CameraShake()
    {
        Vector3 OrginalCamPos = Camera.main.transform.position;
        yield return new WaitForSeconds(0.5f);
        while (CameraShakeTime > 0)
        {
            Camera.main.transform.position = new Vector3(Random.Range(1, 5), Random.Range(1, 5), -10);
        }
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Wall")
        {
            StartCoroutine(CameraShake());
            CameraShakeTime -= Time.deltaTime;
        }
    }

}
