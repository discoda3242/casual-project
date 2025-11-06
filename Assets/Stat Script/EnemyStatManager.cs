// // EnemyStatManager.cs
// using UnityEngine;
// using System;





// public class EnemyStatManager : MonoBehaviour
// {
//     [Header("Templates")]
//     [SerializeField] private EnemyStatData enemyStatData;

//     public float CurrentHP { get; private set; }
//     public int CurrentStage { get; private set; }

//     public event Action<float, float> OnHealthChanged; // (현재HP, 최대HP)
//     public event Action OnDied;

//     private void Awake()
// {
//     if (enemyStatData == null)
//         Debug.LogWarning("[EnemyStatManager] StatData가 비어있습니다.");

//     CurrentStage = 0;
//     CurrentHP = GetMaxHP(CurrentStage);
//     NotifyHPChanged();

// }


//     // ===== 스탯 조회 =====
//     public float GetMaxHP(int stage = -1)
//     {
//         if (enemyStatData == null || enemyStatData.enemies == null || enemyStatData.enemies.Length == 0)
//             return 1f;
//         if (stage < 0) stage = CurrentStage;
//         stage = Mathf.Clamp(stage, 0, enemyStatData.enemies.Length - 1);
//         return enemyStatData.enemies[stage].maxHP;
//     }

//     public float GetAttack(int stage = -1)
//     {
//         if (enemyStatData == null || enemyStatData.enemies == null || enemyStatData.enemies.Length == 0)
//             return 0f;
//         if (stage < 0) stage = CurrentStage;
//         stage = Mathf.Clamp(stage, 0, enemyStatData.enemies.Length - 1);
//         return enemyStatData.enemies[stage].attack;
//     }

//     public float GetDefense(int stage = -1)
//     {
//         if (enemyStatData == null || enemyStatData.enemies == null || enemyStatData.enemies.Length == 0)
//             return 0f;
//         if (stage < 0) stage = CurrentStage;
//         stage = Mathf.Clamp(stage, 0, enemyStatData.enemies.Length - 1);
//         return enemyStatData.enemies[stage].defense;
//     }

//     // ===== 데미지/회복 =====
//     // amount는 "양수 데미지 양"으로 넘겨줘. applyDefense가 true면 방어를 뺀 뒤 최소 1을 보장해.
//     // HP 감소 부분(네 코드 그대로, 마지막 줄은 그대로 둬도 되고 Die()를 직접 호출해도 됨)
// public void HitMonster(float amount, bool applyDefense = false)
// {
//     float before = CurrentHP;

//     float def = applyDefense ? GetDefense(CurrentStage) : 0f;
//     float dmg = Mathf.Max(0f, amount - def);
//     if (applyDefense) dmg = Mathf.Max(1f, dmg);

//     CurrentHP = Mathf.Clamp(CurrentHP - dmg, 0f, GetMaxHP(CurrentStage));
//     NotifyHPChanged();

//     Debug.Log($"[Hit] raw:{amount}, def:{def}, final:{dmg}  HP {before} -> {CurrentHP}");

//     if (CurrentHP <= 0f) OnDied?.Invoke(); // 또는 Die();
// }





//     public void Heal(float amount)
//     {
//         float before = CurrentHP;
//         float add = Mathf.Max(0f, amount);
//         CurrentHP = Mathf.Clamp(CurrentHP + add, 0f, GetMaxHP(CurrentStage));
//         NotifyHPChanged();
//         Debug.Log($"[Heal] +{add}  HP {before} -> {CurrentHP}");
//     }

//     public void FullHeal()
//     {
//         CurrentHP = GetMaxHP(CurrentStage);
//         NotifyHPChanged();
//     }

//     private void NotifyHPChanged()
//     {
//         OnHealthChanged?.Invoke(CurrentHP, GetMaxHP(CurrentStage));
//     }
// }
// EnemyStatManager.cs
// EnemyStatManager.cs
using UnityEngine;
using System;
using System.Collections;

public class EnemyStatManager : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private EnemyStatData enemyStatData;

    public float CurrentHP { get; private set; }
    public int CurrentStage { get; private set; }

    public event Action<float, float> OnHealthChanged; // (현재HP, 최대HP)
    public event Action OnDied;

    [Header("Death")]
    [SerializeField] private float destroyDelay = 0.5f;   // 사망 후 파괴 딜레이
    [SerializeField] private GameObject deathEffect;      // 사망 이펙트(옵션)
    [SerializeField] private Animator animator;           // 사망 트리거(옵션)
    private bool isDead = false;

    private void Awake()
    {
        if (enemyStatData == null)
            Debug.LogWarning("[EnemyStatManager] StatData가 비어있습니다.");

        CurrentStage = 0;                          // 0부터 시작
        CurrentHP = GetMaxHP(CurrentStage);        // 시작 HP
        NotifyHPChanged();

        OnDied += Die;                             // 사망 이벤트 연결
    }

    // ===== 스탯 조회 =====
    public float GetMaxHP(int stage = -1)
    {
        if (enemyStatData == null || enemyStatData.enemies == null || enemyStatData.enemies.Length == 0)
            return 1f;
        if (stage < 0) stage = CurrentStage;
        stage = Mathf.Clamp(stage, 0, enemyStatData.enemies.Length - 1);
        return enemyStatData.enemies[stage].maxHP;
    }

    public float GetAttack(int stage = -1)
    {
        if (enemyStatData == null || enemyStatData.enemies == null || enemyStatData.enemies.Length == 0)
            return 0f;
        if (stage < 0) stage = CurrentStage;
        stage = Mathf.Clamp(stage, 0, enemyStatData.enemies.Length - 1);
        return enemyStatData.enemies[stage].attack;
    }

    public float GetDefense(int stage = -1)
    {
        if (enemyStatData == null || enemyStatData.enemies == null || enemyStatData.enemies.Length == 0)
            return 0f;
        if (stage < 0) stage = CurrentStage;
        stage = Mathf.Clamp(stage, 0, enemyStatData.enemies.Length - 1);
        return enemyStatData.enemies[stage].defense;
    }

    // ===== 데미지/회복 =====
    // amount는 "양수 데미지 양". applyDefense가 true면 방어를 빼고 최소 1 보장.
    public void HitMonster(float amount, bool applyDefense = false)
    {
        float before = CurrentHP;

        float def = applyDefense ? GetDefense(CurrentStage) : 0f;
        float dmg = Mathf.Max(0f, amount - def);
        if (applyDefense) dmg = Mathf.Max(1f, dmg);

        CurrentHP = Mathf.Clamp(CurrentHP - dmg, 0f, GetMaxHP(CurrentStage));
        NotifyHPChanged();

        Debug.Log($"[Hit] raw:{amount}, def:{def}, final:{dmg}  HP {before} -> {CurrentHP}");

        if (CurrentHP <= 0f)
            OnDied?.Invoke();
    }

    public void Heal(float amount)
    {
        float before = CurrentHP;
        float add = Mathf.Max(0f, amount);
        CurrentHP = Mathf.Clamp(CurrentHP + add, 0f, GetMaxHP(CurrentStage));
        NotifyHPChanged();
        Debug.Log($"[Heal] +{add}  HP {before} -> {CurrentHP}");
    }

    public void FullHeal()
    {
        CurrentHP = GetMaxHP(CurrentStage);
        NotifyHPChanged();
    }

    private void NotifyHPChanged()
    {
        OnHealthChanged?.Invoke(CurrentHP, GetMaxHP(CurrentStage));
    }

    // ===== 사망 처리 =====
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        foreach (var c in GetComponentsInChildren<Collider2D>()) c.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider>())   c.enabled = false;
        if (TryGetComponent<Rigidbody2D>(out var rb2d)) rb2d.simulated = false;
        if (TryGetComponent<Rigidbody>(out var rb))     rb.isKinematic = true;

        if (animator) animator.SetTrigger("Die");
        if (deathEffect) Instantiate(deathEffect, transform.position, Quaternion.identity);

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        if (destroyDelay > 0f)
            yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDied -= Die;
    }
}
