using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialCameraController : MonoBehaviour
{
    public GameObject m_PlayerObject;
    public float m_EaseSpeed;
    public float m_CameraFloatHeight;

    public Vector3 m_CurrentVelocity;
    public Vector3 m_PlayerVectors;
    public Vector3 m_CurrentVectors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // We need to do it this way as we need to keep the camera a certain
        // height above everything. If we don't, the camera's clipping planes
        // get involved and the lighting engine freaks out.
        m_PlayerVectors = new Vector3(
            m_PlayerObject.transform.position.x,
            m_CameraFloatHeight,
            m_PlayerObject.transform.position.z
        );

        // Assign the camera's current position to a variable... just to make
        // it cleaner...
        m_CurrentVectors = transform.position;

        transform.position = Vector3.SmoothDamp(m_CurrentVectors, m_PlayerVectors, ref m_CurrentVelocity, m_EaseSpeed);
    }
}
