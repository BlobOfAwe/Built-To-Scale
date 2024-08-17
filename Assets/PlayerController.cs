using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool moving;
    [SerializeField] float moveDuration = 0.5f;
    [SerializeField] float castRange = 300;
    [SerializeField] LayerMask wallMask;

    private Vector2 originTransform;
    [SerializeField] float timer;
    RaycastHit2D targetHit;

    private Collider2D lastPushTile;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
        // If the player is already moving, continue moving
        if (moving) {
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(originTransform, targetHit.point, timer/moveDuration); Debug.Log("Moving"); }

        // If the player is not moving and has input on any axis, move.
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        Debug.Log(moving + " " + transform.position);

    }

    private void LateUpdate()
    {
        // If the player has reached their destination, stop moving
        if (moving && transform.position == toVector3(targetHit.point)) 
        {
            RecenterPosition();
            timer = 0;
            moving = false; Debug.Log("Arrived");

            try
            {
                if (targetHit.collider.CompareTag("PushTile"))
                {
                    Move(lastPushTile.transform.right.x, lastPushTile.transform.right.y);

                }
            }
            catch { }
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

        if (-0.01 >= hInput || 0.01 <= hInput) { pointTo = new Vector2(hInput, 0); } // If there is horizontal input, fire the raycast left or right
        else if (-0.01 >= vInput || 0.01 <= vInput) { pointTo = new Vector2(0, vInput); } // If there is no horizontal input, but there is vertical input, fire raycast up or down
        else { Debug.LogError("Tried to move with no direction!"); return; } // If there is somehow no input, throw an error and return void.

        RaycastHit2D target = Physics2D.CircleCast(transform.position, 0.35f, pointTo, castRange, wallMask); // Fire the circlecast to detect a wall
        try { lastPushTile.enabled = true; } catch { } // Renable the collider of the last push tile contacted.
        
        // If the target is a push tile, disable the tile's collider. This prevents the player from getting stuck on it.
        if (target.collider.CompareTag("PushTile"))
        {
            lastPushTile = target.collider;
            lastPushTile.enabled = false;
        }

        // If the target is a OneWay wall, check if the player is on the correct side of the wall
        // Check this by seeing if the side of the tile with the collider is farther away than the tile's center. If it is, disable the collider and redo the circlecast
        if (target.collider.CompareTag("OneWay") && Vector3.Distance(transform.position, toVector3(target.point)) > Vector3.Distance(transform.position, target.transform.position)) 
        {
            Collider2D oneWayCollider = target.collider;
            oneWayCollider.enabled = false;
            target = Physics2D.CircleCast(transform.position, 0.35f, pointTo, castRange, wallMask); // Fire the circlecast to detect a wall
            oneWayCollider.enabled = true;
        }
        
        // If the raycast hit a wall next to the player, return void
        else if (
            target.point.x < transform.position.x + 1 && target.point.x > transform.position.x - 1 &&
            target.point.y < transform.position.y + 1 && target.point.y > transform.position.y - 1 )
        { moving = false; return; }
        
        targetHit = target; // Record the raycast hit for the Gizmos
        originTransform = transform.position; // log the player's starting position as it moves
    }


    // Utility functions
    static float HalfRound(float num) => (Mathf.FloorToInt(num) * 2 + 1) / 2f; // Round num to the nearest float ending in .5
    static Vector3 toVector3(Vector2 vector) => new Vector3(vector.x, vector.y); // Converts a vector 2 to Vector 3

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetHit.point, 0.35f);
        Gizmos.DrawLine(transform.position, targetHit.point);
    }


}
