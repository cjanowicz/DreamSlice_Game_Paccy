using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;
    public float m_posMultiplier = 1.5f;
    public float m_distance = 10f;        
    public Transform m_Target;


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                 
    private Vector3 m_DesiredPosition;              


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        FindDesiredPosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindDesiredPosition()
    {
        /*Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            averagePos += m_Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos;*/
        m_DesiredPosition = m_Target.position * m_posMultiplier + m_Target.up * m_distance;
    }


    private void Turn()
    {
        transform.LookAt(m_Target);
        
    }


    public void SetStartPositionAndSize()
    {

        transform.position = m_DesiredPosition;
        
    }
}