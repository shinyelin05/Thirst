using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public int magAmount;
    public float fireDelay;
    public int damamge;
    public int skillDamage;
    //10M ���� ź������ ����� ���� ����
    public float actually;
    public float reloadTime;
    public AudioClip fireAudioClip;
    public AudioClip reloadAudioClip;
    public AudioClip skillAudioClip;
    public GameObject impactParticle;
    public GameObject muzzleParticle;
}
