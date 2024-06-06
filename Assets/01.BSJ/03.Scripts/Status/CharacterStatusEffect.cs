using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusEffect : MonoBehaviour
{
    public List<StatusEffect> ActiveStatusEffects { get; private set; } = new List<StatusEffect>();

    public void SetActiveStatusEffects(List<StatusEffect> newStatusEffects)
    {
        ActiveStatusEffects = new List<StatusEffect>(newStatusEffects);
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.ApplyEffect(this, statusEffect.EffectValue);
        ActiveStatusEffects.Add(statusEffect);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.RemoveEffect(this, statusEffect.EffectValue);
        ActiveStatusEffects.Remove(statusEffect);
    }

    public IEnumerator TakeTurn()
    {
        for (int i = ActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            ActiveStatusEffects[i].ApplyEffect(this, ActiveStatusEffects[i].EffectValue);
            ActiveStatusEffects[i].DecreaseDuration();  // Duration--
            if (ActiveStatusEffects[i].Duration <= 0)
            {
                RemoveStatusEffect(ActiveStatusEffects[i]);
            }
            yield return new WaitForSeconds(0.8f);
        }
    }

    public void ResetStatusEffects()
    {
        for (int i = ActiveStatusEffects.Count - 1; i >= 0; i--)
        {
            RemoveStatusEffect(ActiveStatusEffects[i]);
        }
    }

    public void TakeDamage(int damage)
    {
        foreach (var statusEffect in ActiveStatusEffects)
        {
            if (statusEffect.EffectType == StatusEffectType.Poison || statusEffect.EffectType == StatusEffectType.Burn)
            {
                ParticleController.instance.ApplyTargetEffect(statusEffect.ParticleEffect, transform.position, Quaternion.identity, -0.4f);
                SoundManager.instance.PlaySoundEffect(statusEffect.SoundName);
             
                if (gameObject.CompareTag("Monster"))
                {
                    Monster monster = gameObject.GetComponent<Monster>();
                    monster.GetHit(damage);
                }
                else if (gameObject.CompareTag("Player"))
                {
                    Player player = gameObject.GetComponent<Player>();
                    player.GetHit(damage);
                }
            }
        }
    }

    public void Stun()
    {
        // 스턴 처리
    }
}