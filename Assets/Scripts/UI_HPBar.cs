using UnityEngine;

public class UI_HPBar : MonoBehaviour
{
    [SerializeField] private Charater targetCharater; 
    private RectTransform hpBarRect;
    private float maxWidth;

    void Awake()
    {
        hpBarRect = GetComponent<RectTransform>();
        maxWidth = hpBarRect.sizeDelta.x;
    }

    void OnEnable()
    {
        if (targetCharater != null)
            targetCharater.OnHPChanged += UpdateBar;
    }

    void OnDisable()
    {
        if (targetCharater != null)
            targetCharater.OnHPChanged -= UpdateBar;
    }
    void UpdateBar(float currentHP, float maxHP)
    {
        if (hpBarRect != null)
        {
            hpBarRect.sizeDelta = new Vector2((currentHP / maxHP) * maxWidth, hpBarRect.sizeDelta.y);
        }
    }
}
