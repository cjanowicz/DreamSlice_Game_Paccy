using UnityEngine;
using System.Collections;

public class GhostMove : MonoBehaviour {

    public float speed = 0.3f;
    Vector2 dest = Vector2.zero;
    public Transform playerTrans;
    Vector2 target = Vector2.zero;
    //I need Vector2 Dir to have lasting effect
    Vector2 dir = Vector2.up;


    public static GameManager gameManager = null;


    // Use this for initialization
    void Awake () {
        dest = transform.position;
        if(gameManager == null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //if(gameManager.myState 
        target = (Vector2)playerTrans.position;
        //Move closer to Destination
        

        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);
        
        //If we've reached a node
        if ((Vector2)transform.position == dest) {
            //If up isn't going backwards, AND its valid)
            if (Vector2.up != (dir.normalized * -1) && Valid(Vector2.up)) {
                //If up isn't going backwards, AND its valid)
                dest = (Vector2)transform.position + Vector2.up;
            }
            if (Vector2.right != (dir.normalized * -1) && Valid(Vector2.right)) { 
                //if Dest was set by the statement above, then we need to compare
                if(dest != (Vector2)transform.position)
                    dest = ReturnClosest(target, (Vector2)transform.position + Vector2.right, dest);
                //else, we should just set it to this one
                else
                    dest = (Vector2)transform.position + Vector2.right;
            }
            if (Vector2.down != (dir.normalized * -1) && Valid(Vector2.down)) {
                if (dest != (Vector2)transform.position)
                    dest = ReturnClosest(target, (Vector2)transform.position + Vector2.down, dest);
                else
                    dest = (Vector2)transform.position + Vector2.down;
            }
            if (Vector2.left != (dir.normalized * -1) && Valid(Vector2.left)) {
                if (dest != (Vector2)transform.position)
                    dest = ReturnClosest(target, (Vector2)transform.position + Vector2.left, dest);
                else
                    dest = (Vector2)transform.position + Vector2.left;
          
            }
        }
        //Debug.Log("Dest = " + dest + ", dir = " + dir);
        // Animation
        dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);

    }

    void OnTriggerEnter2D(Collider2D co) {
        if (co.name == "pacman")
            Destroy(co.gameObject);
        //TODO:Game Over and Lives system
    }
    bool Valid(Vector2 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        /// If the hit's collider is a collider 2D, then don't move basically.
        /// had to add the (or if its not the maze) because boolean logic, but it works.
        /// So this ghost will run through anything that isn't the maze, including pac-man and the ghosts
        return (hit.collider == GetComponent<Collider2D>() || hit.collider.name != "maze");
    }
    Vector2 ReturnClosest(Vector2 target, Vector2 choice1, Vector2 choice2) {
        if(choice1 == null) {
            return choice2;
        }
        else if(choice2 == null) {
            return choice1;
        } else { 
        if ((target-choice1).magnitude <= (target - choice2).magnitude)
            return choice1;
        else
            return choice2;
        }
    }
}


/*
    public Transform[] waypoints;
    int cur = 0;
// Waypoint not reached yet? then move closer
        if (transform.position != waypoints[cur].position) {
            Vector2 p = Vector2.MoveTowards(transform.position,
                waypoints[cur].position,
                speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else cur = (cur + 1) % waypoints.Length;
*/

/// Note to self: If floating point error happens and ghost is
/// 7.000001, then his raycasting fails to see some maze walls as
/// impassible. Will have to increase that margin of error. 

///Ghost Decision Code
///Tally valid directions
///Go the one that isn't dir*-1
///If he has 2 or more valid directions that aren't dir*-1
///Calculate which square is closer to target(pacman in blinky's case)
///I'm gonna tally up how many valids there are when we reach destination
///then if the valids are >2, compare all valid positions if closer to pacman
///pick closest
///otherwise just go with the only good one