using UnityEngine;
using System.Collections;
using Invector.EventSystems;
using Invector.CharacterController;
using System;

public partial class vCollisionMessage : MonoBehaviour, vIMeleeFighter
{
    public float damageMultiplier = 1f;

    public void OnEnableAttack()
    {

    }

    public void OnDisableAttack()
    {

    }

    public void ResetAttackTriggers()
    {

    }

    public void BreakAttack(int breakAtkID)
    {

    }

    public void OnRecoil(int recoilID)
    {

    }

    public void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
    {
       
        if (!ragdoll) return;      
        if (!ragdoll.iChar.isDead)
        {
            var _damage = new vDamage(damage);
            var value = (float)_damage.damageValue;
            _damage.damageValue = (int)(value * damageMultiplier);
            if (ragdoll.gameObject.IsAMeleeFighter())
                ragdoll.gameObject.GetMeleeFighter().OnReceiveAttack(_damage, attacker);
            else
                ragdoll.gameObject.ApplyDamage(_damage);
        }
    }

    public vCharacter Character()
    {
        return ragdoll.iChar;
    }
}
