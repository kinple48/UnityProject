using UnityEngine;

public class UI_ExpBar : MonoBehaviour
{
    private RectTransform _myRect;
    
    [Header("에디터에서 바의 최대 가로길이를 입력하세요 (예: 500)")]
    [SerializeField] private float initialWidth = 500f; 

    void Awake()
    {
        _myRect = GetComponent<RectTransform>();
        
        // 게임 시작하자마자 바를 0으로 초기화 (깜빡임 방지)
        if (_myRect != null)
            _myRect.sizeDelta = new Vector2(0, _myRect.sizeDelta.y);
    }

    void Start()
    {
        // 모든 Awake가 끝난 Start 시점에 연결을 시도 (싱글톤 null 방지)
        TryConnectToManager();
    }

    void OnEnable()
    {
        // 오브젝트가 껐다 켜질 때도 다시 연결 확인
        TryConnectToManager();
    }

    void TryConnectToManager()
    {
        if (LevelManager.Instance != null)
        {
            // 중복 방지를 위해 기존 구독 해제 후 새로 구독
            LevelManager.Instance.OnExpChanged -= UpdateBar;
            LevelManager.Instance.OnExpChanged += UpdateBar;
            
            // 현재 매니저가 가진 데이터로 즉시 게이지 갱신
            UpdateBar(LevelManager.Instance.currentExp, LevelManager.Instance.nextLevelExp);
        }
        else
        {
            Debug.LogWarning("LevelManager를 찾을 수 없습니다! Hierarchy에 오브젝트가 있는지 확인하세요.");
        }
    }

    void OnDisable()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.OnExpChanged -= UpdateBar;
    }

    void UpdateBar(float currentExp, float maxExp)
    {
        if (_myRect == null) return;

        float ratio = (maxExp > 0) ? currentExp / maxExp : 0;
        float newWidth = ratio * initialWidth;
        
        _myRect.sizeDelta = new Vector2(newWidth, _myRect.sizeDelta.y);
        
        Debug.Log($"[UI] 게이지 갱신: {currentExp}/{maxExp} (폭: {newWidth})");
    }
}
