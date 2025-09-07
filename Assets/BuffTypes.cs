// BuffTypes.cs
public enum StatType { MaxHP, Attack, Defense }
public enum ModKind { Add, Mult } // Add: +x, Mult: ×(1+x)

// 액티브 버프(런타임 상태)
public class ActiveBuff
{
    public StatType targetStat;
    public ModKind modKind;
    public float value;          // Add면 +2, Mult면 +0.2(=+20%)
    public int remainingTurns;   // 필요 없다면 제거 가능

    public ActiveBuff(StatType stat, ModKind kind, float value, int turns = 0)
    {
        targetStat = stat;
        modKind = kind;
        this.value = value;
        remainingTurns = turns;
    }
}