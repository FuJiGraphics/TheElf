using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EffectMap : Singleton<EffectMap>
{
    public Dictionary<string, ParticleSystem> effectMap;

    private void Init()
    {
        if (effectMap != null)
            return;

        var effectsGo = GameObject.FindWithTag("Effects");
        var effects = effectsGo.GetComponentsInChildren<ParticleSystem>();
        if (effects != null)
        {
            effectMap = new Dictionary<string, ParticleSystem>();
        }
        foreach (ParticleSystem effect in effects)
        {
            effectMap.Add(effect.name, effect);
        }
    }

    public void Stop(string effectName)
    {
        this.Init();
        effectMap[effectName]?.Stop();
    }

    public void Play(string effectName, Vector2 pos)
    {
        this.Init();
        effectMap[effectName].transform.position = pos;
        effectMap[effectName]?.Play();
    }

} // class EffectController
