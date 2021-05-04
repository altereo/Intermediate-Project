using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
    public class ParticleSystemMultiplier : MonoBehaviour
    {
        // a simple script to scale the size, speed and lifetime of a particle system

        public float multiplier = 1;
        public ParticleSystem[] m_systems;
        public bool m_CanKill = false;

        private void Start()
        {
            m_systems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in m_systems)
            {
				ParticleSystem.MainModule mainModule = system.main;
				mainModule.startSizeMultiplier *= multiplier;
                mainModule.startSpeedMultiplier *= multiplier;
                mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
                system.Clear();
                system.Play();
            }
        }

        private void Update()
        {
            foreach (ParticleSystem system in m_systems) {
                if (system.IsAlive() == true) {
                    m_CanKill = false;
                    return;
                } else {
                    m_CanKill = true;
                }
            }
            if (m_CanKill) {
                Destroy(gameObject);
            }
        }
    }
}
