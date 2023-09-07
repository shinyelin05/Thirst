using System;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageAble
{
    [SerializeField] protected float maxHp;
    protected float curHp;
    public float Hp { get { return curHp; } set { curHp = value; OnHpChange?.Invoke(); } }
    public float MaxHp { get { return maxHp; } }
    public Action OnHpChange;
    public Action OnFocusIn;
    public Action OnFocusOut;

    public virtual void Awake()
    {
        Hp = maxHp;
    }

    public virtual void Damage(float dmg)
    {
        Hp -= dmg;
        if (Hp < 0)
        {
            Destroy(gameObject, 2.5f);
        }
    }
}
