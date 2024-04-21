using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SlashAnim()
    {
        animator.SetTrigger("Slash");
    }

    public void ChargeAnim()
    {
        animator.SetTrigger("Charge");
    }

    public void DashAnim()
    {
        animator.SetTrigger("Defend");
    }

    public void StabAnim()
    {
        animator.SetTrigger("Stab");
    }

    public void DefendAnim()
    {
        animator.SetTrigger("Defend");
    }

    
}
