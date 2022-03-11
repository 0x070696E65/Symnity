using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particle;
    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }

    public void On()
    {
        particle.Play();
    }
    
    public void Off()
    {
        particle.Stop();
    }
}
