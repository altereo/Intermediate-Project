using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRecycler : MonoBehaviour
{
    // callback for when the particle finishes.
    public void OnParticleSystemStopped()
    {
        gameObject.transform.position = new Vector3(0, -60, 0);
    }
}
