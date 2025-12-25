using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    public int currentLevel = 1;
    public float currentExp = 0;
    public float nextLevelExp = 100;
    public float expMultiplier = 1.2f;

    public event Action<int> OnLevelUp;
    public event Action<float, float> OnExpChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    void Start()
    {
        OnExpChanged?.Invoke(currentExp, nextLevelExp);
    }

    public void AddExp(float amount)
    {
        currentExp += amount;
        Debug.Log($"경험치 획득! 현재: {currentExp} / 목표: {nextLevelExp}");
        while (currentExp >= nextLevelExp)
        {
            LevelUp();
        }

        OnExpChanged?.Invoke(currentExp, nextLevelExp);
    }

    void LevelUp()
    {
        currentExp -= nextLevelExp;
        currentLevel++;
        nextLevelExp *= expMultiplier;
        OnLevelUp?.Invoke(currentLevel);
    }
}
