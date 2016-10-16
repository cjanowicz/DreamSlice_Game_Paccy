using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour {

    public float speed = 0.4f;
    Vector2 dest = Vector2.zero;

    // Use this for initialization
    void Start () {
        dest = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        if ((Vector2)transform.position == dest) {
            if (Input.GetKey(KeyCode.UpArrow) && Valid(Vector2.up))
                dest = (Vector2)transform.position + Vector2.up;
            if (Input.GetKey(KeyCode.RightArrow) && Valid(Vector2.right))
                dest = (Vector2)transform.position + Vector2.right;
            if (Input.GetKey(KeyCode.DownArrow) && Valid(Vector2.down))
                dest = (Vector2)transform.position + Vector2.down;
            if (Input.GetKey(KeyCode.LeftArrow) && Valid(Vector2.left))
                dest = (Vector2)transform.position + Vector2.left;
        }
        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool Valid(Vector2 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>() || hit.collider.name != "maze");
    }
}
