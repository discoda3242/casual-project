// BuffTypes.cs
using UnityEngine;
public class ActiveBuff
{
    public StatType targetStat;
    public ModKind modKind;
    public float value;          // Add면 +2, Mult면 +0.2(=+20%)
    public int remainingTurns;  // 필요 없다면 제거 가능
    public Sprite sp;

    public ActiveBuff(StatType stat, ModKind kind, float value, int turns = 0, Sprite sp = null)
    {
        targetStat = stat;
        modKind = kind;
        this.value = value;
        remainingTurns = turns;
        this.sp = sp;
    }

}