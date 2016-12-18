﻿using UnityEngine;
using System.Collections;

public class PacmanMove3D : MonoBehaviour {

    public float m_speed = 0.4f;
    Vector3 m_dest = Vector3.zero;
    Vector3 m_direction = Vector3.zero;
    int m_timesReachedDestination = 0;

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
        //Move closer to Destination
        Vector3 p = Vector3.MoveTowards(transform.position, m_dest, m_speed);
        GetComponent<Rigidbody>().MovePosition(p);

        if (transform.position == m_dest) {

            Vector3 pos = transform.position;
            RaycastHit hit;

            if (Physics.Raycast(pos, transform.up * -1, out hit, 1.0f)) {
                if (Input.GetKey(KeyCode.UpArrow) && Valid(transform.forward))
                    m_dest = transform.position + transform.forward;
                if (Input.GetKey(KeyCode.RightArrow) && Valid(transform.right))
                    m_dest = transform.position + transform.right;
                if (Input.GetKey(KeyCode.DownArrow) && Valid(transform.forward * -1f))
                    m_dest = transform.position + transform.forward * -1f;
                if (Input.GetKey(KeyCode.LeftArrow) && Valid(transform.right * -1f))
                    m_dest = transform.position + transform.right * -1f;
            } else {
                //Going off a ledge
                //print("No Floor Detected ");

                m_dest = transform.position + (transform.up * -1);

                transform.LookAt(transform.position + Vector3.up, m_direction);
                //Debug.Log("pos  = " + transform.position + ", up*-1 = " + (transform.up * -1) + ", dir = " + m_direction + ", m_dest = " + m_dest);
            }

            m_timesReachedDestination++;
            Debug.Log("timesReachedDestination = " + m_timesReachedDestination);
        }

        m_direction = (m_dest - transform.position).normalized;
    }

    bool Valid(Vector3 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector3 pos = transform.position;
        RaycastHit hit;

        if (Physics.Raycast(pos, dir, out hit, 1.0f)) {
            //print("Found an object - distance: " + hit.distance + ", name is " + hit.collider.tag);
            return (hit.collider.tag != "Wall");
        } else { 
            
            return true;
        }
    }
}