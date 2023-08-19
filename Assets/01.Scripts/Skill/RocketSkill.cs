using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSkill : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    private float damage;

    public void Set(float dmg)
    {
        damage = dmg;
        Destroy(gameObject, 10);
    }

    public void Update()
    {
        speed += Time.deltaTime * 10;
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] damageTargets = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider target in damageTargets)
        {
            if (target.gameObject.TryGetComponent<IDamageAble>(out IDamageAble damageAble))
            {
                damageAble.Damage(damage);
            }
        }

        AudioManager.Instance.PlayAudio(explosionSound);
        Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 4);

        Destroy(gameObject);
    }
}
