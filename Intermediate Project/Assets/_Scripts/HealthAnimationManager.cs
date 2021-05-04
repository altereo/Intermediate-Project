using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAnimationManager : MonoBehaviour
{
    public Animator[] m_HealthAnimators;
    public GameObject[] m_HealthGameObjects;
    public int currentHealthIndex = 4;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure all animators are disabled.
        foreach (Animator healthIcon in m_HealthAnimators) {
            healthIcon.enabled = false;
            healthIcon.Update(0f);
        }
    }

    public void SetHealth(int healthRemaining)
    {
        for (int i = 1; i <= m_HealthAnimators.Length; i++) {
            if (i > healthRemaining) {
                // If we have reached the part of the array we should
                // deactiivate, deactivate them.
                m_HealthAnimators[i - 1].enabled = true;
            }
        }
    }
}
