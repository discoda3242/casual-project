// PlayerStatManager.cs
using UnityEngine;
using System;


public class PlayerStatManager : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private PlayerStatData statData;     // SO에서 기본값을 가져옴

    [Header("Runtime / Buffs")]
    [SerializeField] private BuffManager buffs = new BuffManager();
    public BuffManager Buffs => buffs; // 읽기 전용 접근자

    // 현재 HP는 런타임 상태로 별도 보관
    public float CurrentHP { get; private set; }

    // 이벤트(옵션): 체력 변동 알림이 필요하면 사용
    public event Action<float, float> OnHealthChanged; // (현재HP, 최대HP)
    public event Action OnDied;

    // ---- 초기화 ----
    private void Awake()
    {
        if (statData == null)
        {
            Debug.LogWarning("[PlayerStatManager] StatData가 비어있습니다.");
        }
        // 시작 시 현재 HP = 최대 HP
        CurrentHP = GetMaxHP();
        NotifyHPChanged();
    }

    // ---- 공개 API: 현재 최종 스탯 조회 ----
    public float GetMaxHP()
    {
        float baseVal = statData != null ? statData.baseMaxHP : 100f;
        return Buffs.GetModifiedStat(StatType.MaxHP, baseVal);
    }

    public float GetAttack()
    {
        float baseVal = statData != null ? statData.baseAttack : 10f;
        return Buffs.GetModifiedStat(StatType.Attack, baseVal);
    }

    public float GetDefense()
    {
        float baseVal = statData != null ? statData.baseDefense : 5f;
        return Buffs.GetModifiedStat(StatType.Defense, baseVal);
    }

    // ---- 체력 증감 (데미지/회복 공통) ----
    /// <summary>
    /// delta가 +이면 '데미지', -이면 '회복'으로 처리.
    /// 내부에서 CurrentHP = Clamp(CurrentHP - delta, 0, MaxHP)
    /// </summary>
    public void ApplyHealthDelta(float delta, bool applyDefense = false)
    {
        float finalDelta = delta;

        // 방어력 적용 옵션(원하면 사용)
        if (applyDefense && delta > 0f)
        {
            float def = GetDefense();
            // 간단 예시: 실제데미지 = max(1, delta - def)
            finalDelta = Mathf.Max(1f, delta - def);
        }

        // 체력 갱신
        float maxHP = GetMaxHP();
        CurrentHP = Mathf.Clamp(CurrentHP - finalDelta, 0f, maxHP);//최소 최대값 고정
        NotifyHPChanged();

        // 사망 체크
        if (CurrentHP <= 0f) OnDied?.Invoke();
    }

    // ---- 유틸 ----
    public void FullHeal()
    {
        CurrentHP = GetMaxHP();
        NotifyHPChanged();
    }

    private void NotifyHPChanged()
    {
        OnHealthChanged?.Invoke(CurrentHP, GetMaxHP());
    }

#if UNITY_EDITOR
    // 디버그용 버튼이 필요하면 Odin/EditorGUILayout 등으로 확장 가능
    [ContextMenu("Debug: Take 10 Damage")]
    private void DebugTake10() => ApplyHealthDelta(10f, applyDefense: true);

    [ContextMenu("Debug: Heal 10")]
    private void DebugHeal10() => ApplyHealthDelta(-10f);
#endif
}