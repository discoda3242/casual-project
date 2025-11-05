using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private EnemyStatManager target;   // 대상 슬라임
    [SerializeField] private Slider slider;             // 0~1 범위
    [SerializeField] private TMP_Text hpText;           // "현재/최대" (옵션)
    [SerializeField] private float smoothTime = 0.12f;  // 부드럽게 줄이기

    float targetValue;  // 목표 비율
    float vel;          // SmoothDamp용

    void Awake()
    {
        if (!slider) slider = GetComponentInChildren<Slider>(true);
    }

    void OnEnable()
    {
        if (target)
        {
            target.OnHealthChanged += HandleHealthChanged;
            target.OnDied += OnTargetDied;
            // 초기값 세팅
            HandleHealthChanged(target.CurrentHP, target.GetMaxHP());
        }
    }

    void OnDisable()
    {
        if (target)
        {
            target.OnHealthChanged -= HandleHealthChanged;
            target.OnDied -= OnTargetDied;
        }
    }

    void Update()
    {
        if (!slider) return;
        slider.value = Mathf.SmoothDamp(slider.value, targetValue, ref vel, smoothTime);
    }

    void HandleHealthChanged(float cur, float max)
    {
        max = Mathf.Max(1f, max);
        targetValue = Mathf.Clamp01(cur / max);
        if (hpText) hpText.text = $"{Mathf.CeilToInt(cur)}/{Mathf.CeilToInt(max)}";
    }

    void OnTargetDied()
    {
        Destroy(gameObject); // 적이 죽으면 체력바도 제거
    }

    // 외부에서 타겟을 주입할 때 사용
    public void SetTarget(EnemyStatManager t)
    {
        target = t;
        HandleHealthChanged(t.CurrentHP, t.GetMaxHP());
    }
}
