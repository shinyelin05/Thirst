using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Profiling;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TextMeshPro ammoTmp;
    Weapon curWeapon;

    [Header("HotKeyWeapon")]
    [SerializeField] private List<Weapon> weaponList;

    public void Start()
    {
        foreach (var weapon in weaponList)
        {
            weapon.gameObject.SetActive(false);
        }

        curWeapon = weaponList[0];

        curWeapon.gameObject.SetActive(true);
    }

    public void Update()
    {
        WeaponChange();
        Reload();

        Fire();
        SkillFire();
        UpdateText();
    }

    private void UpdateText()
    {
        ammoTmp.SetText($"{curWeapon.CurAmmo}");
    }

    private void SkillFire()
    {
        if (playerController.isCanShoot)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                curWeapon.SkillFire();
            }
        }
    }

    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            curWeapon.Reload();
        }
    }

    private void Fire()
    {
        if (playerController.isCanShoot)
        {
            curWeapon.inputFire = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            curWeapon.inputFire = false;
        }
    }

    private void WeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponChange(weaponList[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponChange(weaponList[1]);
        }
    }

    private void WeaponChange(Weapon newWeapon)
    {
        if (curWeapon.isReloading)
            return;

        curWeapon.gameObject.SetActive(false);
        newWeapon.gameObject.SetActive(true);

        curWeapon = newWeapon;
    }
}
