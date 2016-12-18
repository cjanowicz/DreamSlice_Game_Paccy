using UnityEngine;
using System.Collections;

public class GhostMove3D : MonoBehaviour {


    public LayerMask m_wallLayer;

    public float m_speed = 0.3f;
    Vector3 m_dest = Vector3.zero;
    public Transform m_playerTrans;
    Vector3 m_target = Vector3.zero;
    //I need Vector3 Dir to have lasting effect
    Vector3 m_dir = Vector3.up;

    public static GameManager m_gameManager = null;


    // Use this for initialization
    void Awake() {
        m_dest = transform.position;
        if (m_gameManager == null)
            m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        //if(gameManager.myState 
        m_target = m_playerTrans.position;
        //Move closer to Destination

        Vector3 p = Vector3.MoveTowards(transform.position, m_dest, m_speed);
        GetComponent<Rigidbody>().MovePosition(p);

        //If we've reached a node
        if (transform.position == m_dest) {

            //If up isn't going backwards, AND its valid)
            if (Vector3.up != (m_dir.normalized * -1) && Valid(Vector3.up)) {
                //If up isn't going backwards, AND its valid)
                m_dest = transform.position + Vector3.up;
            }
            if (Vector3.right != (m_dir.normalized * -1) && Valid(Vector3.right)) {
                //if Dest was set by the statement above, then we need to compare
                if (m_dest != transform.position)
                    m_dest = ReturnClosest(m_target, transform.position + Vector3.right, m_dest);
                //else, we should just set it to this one
                else
                    m_dest = transform.position + Vector3.right;
            }
            if (Vector3.down != (m_dir.normalized * -1) && Valid(Vector3.down)) {
                if (m_dest != transform.position)
                    m_dest = ReturnClosest(m_target, transform.position + Vector3.down, m_dest);
                else
                    m_dest = transform.position + Vector3.down;
            }
            if (Vector3.left != (m_dir.normalized * -1) && Valid(Vector3.left)) {
                if (m_dest != transform.position)
                    m_dest = ReturnClosest(m_target, transform.position + Vector3.left, m_dest);
                else
                    m_dest = transform.position + Vector3.left;
            }
        }
        //Debug.Log("Dest = " + dest + ", dir = " + dir);
        // Animation
        m_dir = m_dest - transform.position;
        GetComponent<Animator>().SetFloat("DirX", m_dir.x);
        GetComponent<Animator>().SetFloat("DirY", m_dir.y);
    }

    void OnTriggerEnter(Collider co) {
        if (co.name == "pacman")
            Destroy(co.gameObject);
        //TODO:Game Over and Lives system
    }
    bool Valid(Vector3 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector3 pos = transform.position;
        //Debug.Log("dir for this call of Valid = " + dir);
        RaycastHit hit;
        Physics.SphereCast(pos, 0.2f, dir, out hit, 1f, m_wallLayer);
        Debug.DrawLine(pos, pos + dir);
        //RaycastHit hit = Physics.Linecast(pos + dir, pos);
        /// If the hit's collider is a collider , then don't move basically.
        /// had to add the (or if its not the maze) because boolean logic, but it works.
        /// So this ghost will run through anything that isn't the maze, including pac-man and the ghosts
        //if (hit.collider != null) {
        //Debug.Log("Valid return = " + (hit.collider != null) + ", name = " + hit.transform.name);
        //}
        return (hit.collider == null /*&& hit.collider.name != "maze"*/);
    }

    Vector3 ReturnClosest(Vector3 target, Vector3 choice1, Vector3 choice2) {
        if (choice1 == null) {
            return choice2;
        } else if (choice2 == null) {
            return choice1;
        } else {
            if ((target - choice1).magnitude <= (target - choice2).magnitude)
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
            GetComponent<Rigidbody>().MovePosition(p);
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