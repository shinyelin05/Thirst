using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO weapondData;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform firePos;
    readonly protected int animatorFire = Animator.StringToHash("Fire");
    protected int curAmmo;
    public int CurAmmo { get { return curAmmo; } }

    [HideInInspector] public bool isReloading;
    [HideInInspector] public bool inputFire;

    protected void Start()
    {
        curAmmo = weapondData.magAmount;
    }

    protected abstract void Fire();
    public abstract void SkillFire();
    public virtual void Reload()
    {
        StartCoroutine(ReloadWeapon());
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(WeaponLifeCycle());
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    protected IEnumerator WeaponLifeCycle()
    {
        while (true)
        {
            yield return new WaitUntil(() => !isReloading);

            if (inputFire && curAmmo > 0)
            {
                curAmmo--;
                Fire();
                yield return new WaitForSeconds(weapondData.fireDelay);
            }
        }
    }
    protected virtual IEnumerator ReloadWeapon()
    {
        AudioManager.Instance.PlayAudio(weapondData.reloadAudioClip);
        isReloading = true;
        yield return new WaitForSeconds(weapondData.reloadTime);
        isReloading = false;
        curAmmo = weapondData.magAmount;
    }
}
