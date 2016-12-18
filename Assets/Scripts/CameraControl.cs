using UnityEngine;

public class CameraControl : MonoBehaviour {
    public float m_DampTime = 0.2f;
    public float m_posMultiplier = 1.5f;
    public float m_distance = 10f;
    public Transform m_Target;
    public float m_lookSpeed = 0.5f;


    private Camera m_Camera;
    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;


    private void Awake() {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate() {
        Move();
        Turn();
    }


    private void Move() {
        FindDesiredPosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindDesiredPosition() {
        m_DesiredPosition = m_Target.position * m_posMultiplier + m_Target.up * m_distance;
    }


    private void Turn() {
        Quaternion targetRotation;
        if (m_Target.up != Vector3.up && m_Target.up * -1 != Vector3.up) {
            //Change the orientation to be world up, but where transform up is set to whatever direction we were last going in

            targetRotation = Quaternion.LookRotation(m_Target.position - transform.position, Vector3.up);
        } else {
            targetRotation = Quaternion.LookRotation(m_Target.position - transform.position, m_Target.forward);
        }

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_lookSpeed * Time.deltaTime);
    }


    public void SetStartPositionAndSize() {

        transform.position = m_DesiredPosition;

    }
}