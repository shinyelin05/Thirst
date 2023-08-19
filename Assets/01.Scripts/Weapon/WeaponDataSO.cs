using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public int magAmount;
    public float fireDelay;
    public int damamge;
    public int skillDamage;
    //10M 에서 탄착군이 만드는 원의 지름
    public float actually;
    public float reloadTime;
    public AudioClip fireAudioClip;
    public AudioClip reloadAudioClip;
    public AudioClip skillAudioClip;
    public GameObject impactParticle;
    public GameObject muzzleParticle;
}
