// BuffManager.cs
using System.Collections.Generic;

public class BuffManager
{
    public readonly List<ActiveBuff> activeBuffs = new();

    public void Add(ActiveBuff b) => activeBuffs.Add(b);

    /// <summary>
    /// baseValue에 현재 활성 버프(해당 스탯)를 모두 반영해 최종값 반환
    /// (Base + Add합) * (1 + Mult합)
    /// </summary>
    public float GetModifiedStat(StatType stat, float baseValue)
    {
        float add = 0f;
        float mult = 0f;

        foreach (var b in activeBuffs)
        {
            if (b.targetStat != stat) continue;
            if (b.modKind == ModKind.Add) add += b.value;
            if (b.modKind == ModKind.Mult) mult += b.value;
        }

        return (baseValue + add) * (1f + mult);
    }

    /// <summary>
    /// 턴 시스템이 있다면 호출해서 만료 처리
    /// </summary>
    public void EndTurn()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            if (activeBuffs[i].remainingTurns <= 0) continue;
            activeBuffs[i].remainingTurns--;
            if (activeBuffs[i].remainingTurns <= 0)
                activeBuffs.RemoveAt(i);
        }
    }
}