using UnityEngine;
using System.Collections;
using Invector.ItemManager;
using Invector.CharacterController;

[vClassHeader("vAmmoStandalone")]
public class vAmmoStandalone : vTriggerGenericAction
{
    [Header("Ammo Standalone Options")]
    [Tooltip("Use the same name as in the AmmoManager")]
    public string weaponName;
    public int ammoAmount;
    private vAmmoManager ammoManager;

    public override IEnumerator OnDoActionDelay(GameObject cc)
    {
        yield return StartCoroutine(base.OnDoActionDelay(cc));

        ammoManager = cc.gameObject.GetComponent<vAmmoManager>();

        var ammo = ammoManager.ammos.Find(_ammo => _ammo.weaponName.Equals(weaponName));
        ammo.count += ammoAmount;
        ammoManager.UpdateTotalAmmo();
    }
}