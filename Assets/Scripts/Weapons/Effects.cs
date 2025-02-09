using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public ParticleSystem[] effects;

    private void Start()
    {
        effects = GetComponentsInChildren<ParticleSystem>();
    }

    public void Play(Vector2 position, Quaternion rotation)
    {
        foreach (ParticleSystem p in effects)
        {
            p.transform.position = position;
            p.transform.rotation = rotation;
            p.Play();
        }
    }

    public void Stop()
    {
        foreach (ParticleSystem p in effects)
        {
            p.Stop();
        }
    }

} // class Effects
