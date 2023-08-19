using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    private Camera cam;
    private Camera Cam { get { if (cam == null) { cam = Camera.main; } return cam; } }

    [SerializeField] private RocketSkill rocket;

    public override void SkillFire()
    {
        AudioManager.Instance.PlayAudio(weapondData.skillAudioClip);
        RocketSkill skill = Instantiate(rocket, firePos.position, firePos.rotation);
        skill.Set(weapondData.skillDamage);
    }

    protected override void Fire()
    {
        AudioManager.Instance.PlayAudio(weapondData.fireAudioClip);
        animator.SetTrigger(animatorFire);

        GameObject obj = Instantiate(weapondData.muzzleParticle, firePos.position, firePos.rotation);
        obj.transform.SetParent(firePos);
        Destroy(obj, 3);

        for (int i = 0; i < 8; i++)
        {
            Vector3 randPos = Cam.transform.position + Cam.transform.forward * 10 + Cam.transform.TransformDirection(Random.insideUnitCircle * weapondData.actually * 0.5f);
            Vector3 dir = (randPos - Cam.transform.position).normalized;

            if (Physics.Raycast(Cam.transform.position, dir, out RaycastHit hit))
            {
                obj = Instantiate(weapondData.impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(obj, 3);
                if (hit.transform.TryGetComponent<IDamageAble>(out IDamageAble damageAble))
                {
                    damageAble.Damage(weapondData.damamge);
                }
            }
        }
    }
}
