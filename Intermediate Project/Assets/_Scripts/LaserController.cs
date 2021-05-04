using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Animator m_LaserAnimator;
    public bool m_LaserCanFire = true;
    public float m_LaserAnimationTime;

    private void Awake()
    {
        // Store the animation's length in ms as a float.
        m_LaserAnimationTime = m_LaserAnimator.runtimeAnimatorController.animationClips[0].length * 1000;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_LaserAnimator.gameObject.GetComponent<MeshRenderer>().enabled = false;
        m_LaserAnimator.enabled = false;
        m_LaserAnimator.Update(0f);
        m_LaserCanFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        // More debug stuff to fire the laser.
        if (Application.isEditor) {
            if (Input.GetKeyDown(KeyCode.Period)) {
                FireLaser(1000);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            var playerController = collision.gameObject.GetComponent<PlayerGridlockController>();
            if (playerController) {
                playerController.HurtPlayer(1);
            }
        }
    }

    public void FireLaser(float laserTimeMs)
    {
        if (m_LaserCanFire) {
            var newSpeed = m_LaserAnimationTime / laserTimeMs;
            m_LaserAnimator.speed = newSpeed;
            m_LaserCanFire = false;
            m_LaserAnimator.gameObject.GetComponent<MeshRenderer>().enabled = true;
            m_LaserAnimator.enabled = true;
            m_LaserAnimator.Update(0f);
            // Fire the laser.
            m_LaserAnimator.Play("laserAnimator", -1, 0f);
        } else {
            Debug.LogError("Tried to fire laser \"" + gameObject.name + "\" when it is unavailable");
        }
        
    }

    public void AlertObservers(string message)
    {
        if (message == "LFinished") {
            m_LaserAnimator.gameObject.GetComponent<MeshRenderer>().enabled = false;
            m_LaserAnimator.Update(0f);
            m_LaserAnimator.enabled = false;
            m_LaserCanFire = true;
        }
    }
}
