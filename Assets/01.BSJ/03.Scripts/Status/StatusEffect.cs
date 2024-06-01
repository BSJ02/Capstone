using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectType
{
    Poison,
    Burn,
    Stun
    // 다른 상태이상 추가
}

public class StatusEffect
{
    public StatusEffectType EffectType { get; private set; }
    public int Duration { get; private set; }
    public int EffectValue { get; private set; }
    public Action<CharacterStatusEffect, int> ApplyEffect { get; private set; }
    public Action<CharacterStatusEffect, int> RemoveEffect { get; private set; }
    public GameObject ParticleEffect { get; private set; }

    public StatusEffect(StatusEffectType effectType, int duration, int effectValue,
                Action<CharacterStatusEffect, int> applyEffect, Action<CharacterStatusEffect, int> removeEffect, GameObject particleEffect)
    {
        EffectType = effectType;
        Duration = duration;
        EffectValue = effectValue;
        ApplyEffect = applyEffect;
        RemoveEffect = removeEffect;
        ParticleEffect = particleEffect;
    }

    public void DecreaseDuration()
    {
        Duration--;
    }
}