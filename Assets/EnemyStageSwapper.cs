using UnityEngine;
using UnityEngine.UI; // UI 이미지 아이콘을 바꿀 때만 필요

public class EnemyStageSwapper : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EnemyStatData statData;

    [Header("Target Renderers (둘 중 하나만 써도 됨)")]
    [SerializeField] private SpriteRenderer spriteRenderer; // 월드 스프라이트
    [SerializeField] private Image uiImage;                 // UI 이미지(아이콘)

    [Header("State")]
    [SerializeField] private int currentStage = 0;
    public float CurrentHP { get; private set; }

    public System.Action<float, float> OnHealthChanged; // (현재HP, 최대HP)
    public System.Action<int, EnemyStat> OnStageChanged;

    private void Awake()
    {
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyStage(currentStage); // 시작 스테이지 적용
    }

    public void TakeDamage(float dmg)
    {
        var entry = statData.GetEntry(currentStage);
        if (entry == null) return;

        // 간단한 방어 계산(원하면 빼고 데미지 그대로 써도 됨)
        float final = Mathf.Max(0f, dmg - entry.defense);
        CurrentHP -= final;
        OnHealthChanged?.Invoke(Mathf.Max(CurrentHP, 0f), entry.maxHP);

        if (CurrentHP <= 0f)
        {
            NextStage(); // **죽으면 다음 스테이지로** (스폰 없이 교체)
        }
    }

    public void NextStage()
    {
        int next = Mathf.Min(currentStage + 1, statData.StageCount - 1);
        if (next == currentStage) return; // 마지막 스테이지면 멈춤(원하면 클리어 이벤트 추가)

        ApplyStage(next);
    }

    public void ApplyStage(int stage)
    {
        currentStage = Mathf.Clamp(stage, 0, statData.StageCount - 1);
        var entry = statData.GetEntry(currentStage);
        if (entry == null) return;

        // 스탯 적용
        CurrentHP = entry.maxHP;
        OnHealthChanged?.Invoke(CurrentHP, entry.maxHP);
        OnStageChanged?.Invoke(currentStage, entry);

        // PNG 교체 (어디에 그릴지에 따라 둘 중 하나/둘 다 적용)
        if (entry.icon != null)
        {
            if (spriteRenderer) spriteRenderer.sprite = entry.icon; // 월드 스프라이트 교체
            if (uiImage) uiImage.sprite = entry.icon;               // UI 아이콘 교체
        }

        // 필요하면 콜라이더 / 크기 보정 등 여기에 처리
        // e.g., GetComponent<BoxCollider2D>()?.size = spriteRenderer.sprite.bounds.size;
    }
}
