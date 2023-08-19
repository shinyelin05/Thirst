using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    private Camera cam;
    private Camera Cam { get { if (cam == null) { cam = Camera.main; } return cam; } }
    [SerializeField] private LaserSkill laserSkill;

    public override void SkillFire()
    {
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit))
        {
            LaserSkill skill = Instantiate(laserSkill);
            skill.Set(firePos.position, hit.point);

            GameObject obj = Instantiate(weapondData.impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
            obj.transform.localScale = Vector3.one * 4;
            Destroy(obj, 3);

            if (hit.transform.TryGetComponent<IDamageAble>(out IDamageAble damageAble))
            {
                damageAble.Damage(weapondData.damamge);
            }
        }
    }

    protected override void Fire()
    {
        AudioManager.Instance.PlayAudio(weapondData.fireAudioClip);
        animator.SetTrigger(animatorFire);

        Vector3 randPos = Cam.transform.position + Cam.transform.forward * 10 + Cam.transform.TransformDirection(Random.insideUnitCircle * weapondData.actually * 0.5f);
        Vector3 dir = (randPos - Cam.transform.position).normalized;

        if (Physics.Raycast(Cam.transform.position, dir, out RaycastHit hit))
        {
            GameObject obj = Instantiate(weapondData.impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(obj, 3);

            obj = Instantiate(weapondData.muzzleParticle, firePos.position, firePos.rotation);
            obj.transform.SetParent(firePos);
            Destroy(obj, 3);

            if(hit.transform.TryGetComponent<IDamageAble>(out IDamageAble damageAble))
            {
                damageAble.Damage(weapondData.damamge);
            }
        }
    }
}
