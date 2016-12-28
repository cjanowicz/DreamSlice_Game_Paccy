using UnityEngine;
using System.Collections;

public class PacmanMove3D : MonoBehaviour {

    public float m_speed = 0.4f;
    Vector3 m_dest = Vector3.zero;
    Vector3 m_direction = Vector3.zero;
    public LayerMask m_wallLayer;

    // Use this for initialization
    void Start() {
        m_dest = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();

        //GetComponent<Animator>().SetFloat("DirX", m_direction.x);
        //GetComponent<Animator>().SetFloat("DirY", m_direction.y);
    }

    void Move() {
        
        if (transform.position == m_dest) {
            Vector3 pos = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(pos, transform.up * -1, out hit, 1.0f, m_wallLayer)) {
                if (Input.GetKey(KeyCode.UpArrow) && Valid(transform.forward))
                    m_dest = transform.position + transform.forward;
                if (Input.GetKey(KeyCode.RightArrow) && Valid(transform.right))
                    m_dest = transform.position + transform.right;
                if (Input.GetKey(KeyCode.DownArrow) && Valid(transform.forward * -1f))
                    m_dest = transform.position + transform.forward * -1f;
                if (Input.GetKey(KeyCode.LeftArrow) && Valid(transform.right * -1f))
                    m_dest = transform.position + transform.right * -1f;
            } else {
                ///Going off a ledge
                ///Move Down
                m_dest = transform.position + (transform.up * -1);
                Vector3 perpendicular = Vector3.Cross(transform.up, m_direction);
                transform.Rotate(perpendicular, 90, Space.World);
            }
        }
        m_direction = (m_dest - transform.position).normalized;
        //Move closer to Destination
        Vector3 p = Vector3.MoveTowards(transform.position, m_dest, m_speed);
        GetComponent<Rigidbody>().MovePosition(p);
    }

    bool Valid(Vector3 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector3 pos = transform.position;
        RaycastHit hit;

        if (Physics.Raycast(pos, dir, out hit, 1.0f, m_wallLayer)) {
            return (hit.collider.tag != "Wall");
        } else {
            return true;
        }
    }
}