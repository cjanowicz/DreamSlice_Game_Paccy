using UnityEngine;
using System.Collections;

public class GhostMove3D : MonoBehaviour {


    public LayerMask m_wallLayer;

    public float m_speed = 0.3f;
    Vector3 m_dest = Vector3.zero;
    public Transform m_playerTrans;
    Vector3 m_target = Vector3.zero;
    //I need Vector3 Dir to have lasting effect
    Vector3 m_dir = Vector3.forward;

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

            Vector3 pos = transform.position;
            RaycastHit hit;

            if (Physics.Raycast(pos, transform.up * -1, out hit, 1.0f, m_wallLayer)) {

                //If up isn't going backwards, AND its valid)
                if (transform.forward != (m_dir * -1) && Valid(transform.forward)) {
                    //If up isn't going backwards, AND its valid)
                    m_dest = transform.position + transform.forward;
                }
                if (transform.right != (m_dir * -1) && Valid(transform.right)) {
                    //if Dest was set by the statement above, then we need to compare
                    if (m_dest != transform.position)
                        m_dest = ReturnClosest(m_target, transform.position + transform.right, m_dest);
                    //else, we should just set it to this one
                    else
                        m_dest = transform.position + transform.right;
                }
                Vector3 back = transform.forward * -1;
                if (back != (m_dir * -1) && Valid(back)) {
                    if (m_dest != transform.position)
                        m_dest = ReturnClosest(m_target, transform.position + back, m_dest);
                    else
                        m_dest = transform.position + back;
                }
                Vector3 left = transform.right * -1;
                if (left != (m_dir * -1) && Valid(left)) {
                    if (m_dest != transform.position)
                        m_dest = ReturnClosest(m_target, transform.position + left, m_dest);
                    else
                        m_dest = transform.position + left;
                }

            } else {
                //Going off a ledge
                //print("No Floor Detected ");

                //Move Down
                m_dest = transform.position + (transform.up * -1);

                if (m_dir != Vector3.up && m_dir * -1 != Vector3.up) {
                    //Change the orientation to be world up, but where transform up is set to whatever direction we were last going in
                    transform.LookAt(transform.position + Vector3.up, m_dir);
                } else {
                    transform.LookAt(transform.position + Vector3.forward, m_dir);
                }
                //Debug.Log("pos  = " + transform.position + ", up*-1 = " + (transform.up * -1) + ", dir = " + m_direction + ", m_dest = " + m_dest);
            }

        }
        //Debug.Log("Dest = " + dest + ", dir = " + dir);
        // Animation
        m_dir = (m_dest - transform.position).normalized;
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