using UnityEngine;
using System;

public class Charater : MonoBehaviour
{
    public float MaxHP = 3;
    public float HP { get; private set; }
    
    public event Action<float, float> OnHPChanged;

    public void Initialize()
    {
        HP = MaxHP;
        OnHPChanged?.Invoke(HP, MaxHP);
    }

    public bool Hit(float damage)
    {
        HP = Mathf.Max(0, HP - damage);
        
        OnHPChanged?.Invoke(HP, MaxHP);
        
        return HP > 0;
    }
}
