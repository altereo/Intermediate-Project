using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera[] m_cameraArray = null;
    public int m_SelectedCameraIndex = 0;
    public GameManager m_gameManager;

    private void Start()
    {
        // When we get in, reset the cameras, just in case.
        FullResetCameraView();
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow switching cameras if we're actually playing.
        if (m_gameManager.State == GameManager.GameState.Playing) {
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) {
                if (m_cameraArray.Length > 1) { // Don't even bother if we've only got one camera... there's no point.
                    if (m_SelectedCameraIndex != m_cameraArray.Length - 1) {
                        // Set the currently active camera and audio listener to inactive.
                        DisableEnableCamera(m_cameraArray[m_SelectedCameraIndex], false);

                        // Get the next camera in the list and set it to active.
                        m_SelectedCameraIndex += 1;
                        DisableEnableCamera(m_cameraArray[m_SelectedCameraIndex], true);
                    } else {
                        // If we're at the end of the list, loop back around to 0 and disable the camera at the end of the list.
                        m_SelectedCameraIndex = 0;
                        DisableEnableCamera(m_cameraArray[0], true);
                        DisableEnableCamera(m_cameraArray[m_cameraArray.Length - 1], false);
                    }
                }
                
            }
        }
    }

    private void DisableEnableCamera(Camera selectedCamera, bool state)
    {
        selectedCamera.enabled = state;
        selectedCamera.GetComponentInParent<AudioListener>().enabled = state;
    }

    public void FullResetCameraView()
    {
        // FUlly resets the camera view (disables all of them first.);
        
        // First disable them all.
        foreach (Camera cameraToDeactivate in m_cameraArray) {
            DisableEnableCamera(cameraToDeactivate, false);
        }

        // Now enable the first one.
        m_SelectedCameraIndex = 0;
        DisableEnableCamera(m_cameraArray[0], true);
    }

    public void ResetCameraView()
    {
        // Resets the camera to the first one...
        // We don't need to do anything here if we've already got
        // the first camera selected.
        if (m_SelectedCameraIndex != 0) {
            DisableEnableCamera(m_cameraArray[m_SelectedCameraIndex], false);
            DisableEnableCamera(m_cameraArray[0], true);
            m_SelectedCameraIndex = 0;
        }
    }
}
