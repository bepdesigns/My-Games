﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Invector;
using Invector.ItemManager;
using Invector.CharacterController;

public class vShooterManager : vMonoBehaviour
{
    #region variables

    [System.Serializable]
    public class OnReloadWeapon : UnityEngine.Events.UnityEvent<vShooterWeapon> { }
    public delegate void TotalAmmoHandler(int ammoID, ref int ammo);
    [Tooltip("min distance to aim")]
    public float minDistanceToAim = 1;
    public float checkAimRadius = 0.1f;
    [Tooltip("smooth of the right hand when correcting the aim")]
    public float smoothHandRotation = 20f;
    [Tooltip("Limit the maxAngle for the right hand to correct the aim")]
    public float maxHandAngle = 60f;
    [Tooltip("Check true to make the character always aim and walk on strafe mode")]
    public bool alwaysAiming;
    [Tooltip("Check this to syinc the weapon aim to the camera aim")]
    public bool raycastAimTarget = true;
    [Tooltip("Allow the use of the LockOn or not")]
    public bool useLockOn = false;
    [Tooltip("Allow the use of the LockOn only with a Melee Weapon")]
    public bool useLockOnMeleeOnly = true;
    [Tooltip("Use the vAmmoDisplay to shot ammo count")]
    public bool useAmmoDisplay = true;
    [Tooltip("Move camera angle when shot using recoil properties of weapon")]
    public bool applyRecoilToCamera = true;
    [Tooltip("Check this to use IK on the left hand")]
    public bool useLeftIK = true;
    [Tooltip("Instead of adjust each weapon individually, make a single offset here for each character")]
    public Vector3 ikRotationOffset;
    [Tooltip("Instead of adjust each weapon individually, make a single offset here for each character")]
    public Vector3 ikPositionOffset;
    [Tooltip("Layer to aim down")]
    public LayerMask blockAimLayer = 1 << 0;
    [Tooltip("Layer to aim")]
    public LayerMask aimTargetLayer = 1 << 0;
    [Tooltip("Tags to the Aim ignore - tag this gameObject to avoid shot on yourself")]
    public List<string> ignoreTags;


    //[HideInInspector]
    public vShooterWeapon rWeapon;
    [HideInInspector]
    public vAmmoManager ammoManager;
    public TotalAmmoHandler totalAmmoHandler;
    [HideInInspector]
    public bool shotting;
    [HideInInspector]
    public OnReloadWeapon onReloadWeapon;
    [HideInInspector]
    public vAmmoDisplay ammoDisplay;
    [HideInInspector]
    public vThirdPersonCamera tpCamera;
    [HideInInspector]
    public bool showCheckAimGizmos;

    private Animator animator;
    private int totalAmmo;
    private bool usingThirdPersonController;
    #endregion

    void Start()
    {
        animator = GetComponent<Animator>();
        if (applyRecoilToCamera)
            tpCamera = FindObjectOfType<vThirdPersonCamera>();
        ammoManager = GetComponent<vAmmoManager>();
        usingThirdPersonController = GetComponent<vThirdPersonController>() != null;
        if (useAmmoDisplay)
            ammoDisplay = FindObjectOfType<vAmmoDisplay>();

        if (animator)
        {
            var _rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            var weapon = _rightHand.GetComponentInChildren<vShooterWeapon>();
            if (weapon != null)
                SetRightWeapon(weapon.gameObject);
        }

        if (!ignoreTags.Contains(gameObject.tag))
            ignoreTags.Add(gameObject.tag);

        if (useAmmoDisplay && ammoDisplay)
            ammoDisplay.UpdateDisplay("");
    }

    public void SetRightWeapon(GameObject weapon)
    {
        if (weapon != null)
        {
            var w = weapon.GetComponent<vShooterWeapon>();
            rWeapon = w;
            if (rWeapon)
            {
                rWeapon.ignoreTags = ignoreTags;
                rWeapon.hitLayer = aimTargetLayer;
                rWeapon.root = transform;
                rWeapon.onDestroy.AddListener(OnDestroyWeapon);
                if (usingThirdPersonController)
                {
                    if (useAmmoDisplay && !ammoDisplay) ammoDisplay = FindObjectOfType<vAmmoDisplay>();
                    if (useAmmoDisplay && ammoDisplay) ammoDisplay.Show();
                    UpdateTotalAmmo();
                }
            }
        }
    }

    public void OnDestroyWeapon(GameObject otherGameObject)
    {
        if (usingThirdPersonController)
        {
            if (useAmmoDisplay && !ammoDisplay) ammoDisplay = FindObjectOfType<vAmmoDisplay>();
            if (useAmmoDisplay && (ammoDisplay && (rWeapon == null || rWeapon.gameObject.Equals(otherGameObject))))
            {
                ammoDisplay.UpdateDisplay("");
                ammoDisplay.Hide();
            }
        }

    }

    public int GetMoveSetID()
    {
        int id = 0;
        if (rWeapon) id = (int)rWeapon.moveSetID;
        return id;
    }

    public int GetUpperBodyID()
    {
        int id = 0;
        if (rWeapon) id = (int)rWeapon.upperBodyID;
        return id;
    }

    public int GetAttackID()
    {
        int id = 0;
        if (rWeapon) id = (int)rWeapon.attackID;
        return id;
    }

    public int GetEquipID()
    {
        int id = 0;
        if (rWeapon) id = (int)rWeapon.equipID;
        return id;
    }

    public int GetReloadID()
    {
        int id = 0;
        if (rWeapon) id = (int)rWeapon.reloadID;
        return id;
    }

    public virtual bool WeaponHasAmmo()
    {
        return totalAmmo > 0;
    }

    public void ReloadWeapon()
    {
        if (rWeapon == null) return;
        UpdateTotalAmmo();
        if (rWeapon.ammoCount >= rWeapon.clipSize || !WeaponHasAmmo()) return;
        onReloadWeapon.Invoke(rWeapon);
        var needAmmo = rWeapon.clipSize - rWeapon.ammoCount;
        if (WeaponAmmo().count < needAmmo)
            needAmmo = WeaponAmmo().count;

        rWeapon.AddAmmo(needAmmo);
        WeaponAmmo().count -= needAmmo;
        if (animator)
        {
            animator.SetInteger("ReloadID", GetReloadID());
            animator.SetTrigger("Reload");
        }

        rWeapon.ReloadEffect();
        UpdateTotalAmmo();
    }

    public vAmmo WeaponAmmo()
    {
        if (rWeapon == null) return null;

        var ammo = new vAmmo();
        if (ammoManager && ammoManager.ammos != null && ammoManager.ammos.Count > 0)
        {
            ammo = ammoManager.ammos.Find(a => a.ammoID == rWeapon.ammoID);
        }

        return ammo;
    }

    public void UpdateTotalAmmo()
    {
        var ammoCount = 0;
        var ammo = WeaponAmmo();
        if (ammo != null) ammoCount += ammo.count;
        if (totalAmmoHandler != null) totalAmmoHandler(rWeapon.ammoID, ref ammoCount);
        totalAmmo = ammoCount;
        if (useAmmoDisplay && !ammoDisplay) ammoDisplay = FindObjectOfType<vAmmoDisplay>();

        if (useAmmoDisplay && ammoDisplay)
            ammoDisplay.UpdateDisplay(string.Format("{0} / {1}", rWeapon.ammoCount, totalAmmo), rWeapon.ammoID);
    }

    public virtual void Shoot(Vector3 aimPosition)
    {
        if (rWeapon.ammoCount > 0)
        {
            shotting = true;
            rWeapon.ShootEffect(aimPosition, transform);
            if (applyRecoilToCamera)
            {
                var recoilHorizontal = Random.Range(rWeapon.recoilLeft, rWeapon.recoilRight);
                var recoilUp = Random.Range(0, rWeapon.recoilUp);
                StartCoroutine(Recoil(recoilHorizontal, recoilUp));
            }
            if (animator) animator.SetTrigger("Shoot");
            StartCoroutine(ResetShot(rWeapon.shootFrequency));
            if (useAmmoDisplay && !ammoDisplay) ammoDisplay = FindObjectOfType<vAmmoDisplay>();
            if (useAmmoDisplay && ammoDisplay)
                ammoDisplay.UpdateDisplay(string.Format("{0} / {1}", rWeapon.ammoCount, totalAmmo), rWeapon.ammoID);
        }
        else
        {
            shotting = true;
            rWeapon.EmptyClipEffect();
            StartCoroutine(ResetShot(rWeapon.shootFrequency));
        }
    }

    IEnumerator Recoil(float horizontal, float up)
    {
        yield return new WaitForSeconds(0.02f);
        if (tpCamera != null) tpCamera.RotateCamera(horizontal, up);
    }

    IEnumerator ResetShot(float time)
    {
        yield return new WaitForSeconds(time);
        shotting = false;
    }
}

namespace Invector
{
    [System.Serializable]
    public class vAmmo
    {
        [Tooltip("Use the same name of your weapon")]
        public string weaponName;
        [Tooltip("Ammo ID - make sure your AmmoManager and ItemListData use the same ID")]
        public int ammoID;
        [Tooltip("Don't need to setup if you're using a Inventory System")]
        public int count;
        public vAmmo()
        {

        }
        public vAmmo(string weaponName, int ammoID, int count)
        {
            this.weaponName = weaponName;
            this.ammoID = ammoID;
            this.count = count;
        }
        public vAmmo(vAmmo ammo)
        {
            this.weaponName = ammo.weaponName;
            this.ammoID = ammo.ammoID;
            this.count = ammo.count;
        }
    }
}
