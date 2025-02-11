using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public void Stop()
        => GetComponent<ParticleSystem>()?.Stop();

    public void Play()
        => GetComponent<ParticleSystem>()?.Play();

} // class EffectController
