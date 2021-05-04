using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody m_PlayerRigidBody; // A field for the player's rigidbody.
    public GameObject m_CameraGameObject; // A field for the camera's gameobject.
    public float m_CameraSensitivity; // How fast to rotate the camera

    public bool m_IsCameraControllable = true;

    public Vector2 m_MouseMovementAxis = new Vector2(0f, 0f); // The place to store where the mouse is moving.
    public float m_YAxisTurnBy = 0f;
    public float m_XAxisTurnBy = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsCameraControllable) {
            // Lock and hide the cursor.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Store the mouse axis-es into a variable.
            m_MouseMovementAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            // Setup the angles.
            m_YAxisTurnBy = m_MouseMovementAxis.x * m_CameraSensitivity * Time.deltaTime;
            m_XAxisTurnBy = m_MouseMovementAxis.y * -1f * m_CameraSensitivity * Time.deltaTime;

            // Trigger the rotations.
            Rotate();
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }

    void Rotate()
    {
        // Rotate the camera around the y axis.
        m_CameraGameObject.transform.Rotate(0f, m_YAxisTurnBy, 0f, Space.World);

        // Rotate the camera around the x axis.
        m_CameraGameObject.transform.Rotate(m_XAxisTurnBy, 0f, 0f, Space.Self);
    }
}
