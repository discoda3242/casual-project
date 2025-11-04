// EnemyStatManager.cs
using UnityEngine;
using System;


public class EnemyStatManager : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private EnemyStatData enemyStatData;     // SO에서 기본값을 가져옴

    // 현재 HP는 런타임 상태로 별도 보관
    public float CurrentHP { get; private set; }
    public float CurrentStage { get; private set; }

    // 이벤트(옵션): 체력 변동 알림이 필요하면 사용
    public event Action<float, float> OnHealthChanged; // (현재HP, 최대HP)
    public event Action OnDied;

    // ---- 초기화 ----
    private void Awake()
    {
        if (enemyStatData == null)
        {
            Debug.LogWarning("[EnemyStatManager] StatData가 비어있습니다.");
        }
    //     시작 시 현재 HP = 최대 HP
        CurrentHP = GetMaxHP();
        CurrentStage = 1;
        NotifyHPChanged();
    }

    // ---- 공개 API: 현재 최종 스탯 조회 ----
    public float GetMaxHP(int stage=1)
    {
        return enemyStatData.enemies[stage].maxHP;
    }

    public float GetAttack(int stage=1)
    {
        return enemyStatData.enemies[stage].attack;

    }

    public float GetDefense(int stage=1)
    {
        return enemyStatData.enemies[stage].defense;
    }

    // ---- 체력 증감 (데미지/회복 공통) ----
    /// <summary>
    /// delta가 +이면 '데미지', -이면 '회복'으로 처리.
    /// 내부에서 CurrentHP = Clamp(CurrentHP - delta, 0, MaxHP)
    /// </summary>
    public void HitMonster(float delta, bool applyDefense = false)
    {
        float finalDelta = delta;
        if(applyDefense)
        {
            finalDelta -= GetDefense();
        }
        float maxHP = GetMaxHP();
        CurrentHP = Mathf.Clamp(CurrentHP - finalDelta, 0f, maxHP);
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


}