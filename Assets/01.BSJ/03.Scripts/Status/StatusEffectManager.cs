using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    public static StatusEffectManager instance;

    private List<CharacterStatusEffect> Characters_Player;
    private List<CharacterStatusEffect> Characters_Monster;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        Characters_Player = new List<CharacterStatusEffect>();
        Characters_Monster = new List<CharacterStatusEffect>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Characters_Player.Add(player.GetComponent<CharacterStatusEffect>());
        }

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject monster in monsters)
        {
            Characters_Monster.Add(monster.GetComponent<CharacterStatusEffect>());
        }
    }

    public void ApplyPoisonEffect(CharacterStatusEffect target)
    {
        var poisonEffect = new StatusEffect(
            StatusEffectType.Poison,    // Type
            3,  // Duration
            10, // Damage
            (type, value) => type.TakeDamage(value),
            (type, value) => { },
            ParticleController.instance.fireballEffectPrefab,
            "Burn"
        );

        target.ApplyStatusEffect(poisonEffect);
    }

    public void ApplyBurnEffect(CharacterStatusEffect target)
    {
        var burnEffect = new StatusEffect(
            StatusEffectType.Burn,    // Type
            2,  // Duration
            20, // Damage
            (type, value) => type.TakeDamage(value),
            (type, value) => { },
            ParticleController.instance.fireballEffectPrefab,
            "Burn"
        );

        target.ApplyStatusEffect(burnEffect);
    }

    private void ApplyStunEffect(CharacterStatusEffect target)
    {
        var stunEffect = new StatusEffect(
            StatusEffectType.Stun,  // Type
            1,  // Duration
            0,  // Damage
            (type, value) => type.Stun(),
            (type, value) => { },
            ParticleController.instance.fireballEffectPrefab,
            "Burn"
        );

        target.ApplyStatusEffect(stunEffect);
    }

    public IEnumerator PlayerTurnSimulation()
    {
        foreach (CharacterStatusEffect character in Characters_Player)
        {
            yield return StartCoroutine(character.TakeTurn());
        }

        yield return new WaitForSeconds(0.8f);
    }

    public IEnumerator MonsterTurnSimulation(GameObject monsterObj)
    {
        CharacterStatusEffect character = monsterObj.GetComponent<CharacterStatusEffect>();

        yield return StartCoroutine(character.TakeTurn());

        yield return new WaitForSeconds(0.8f);
    }
}