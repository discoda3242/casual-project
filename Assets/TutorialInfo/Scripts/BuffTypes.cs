// BuffTypes.cs
using UnityEngine;
public class ActiveBuff
{
    public StatType targetStat;
    public ModKind modKind;
    public float value;          // Add�� +2, Mult�� +0.2(=+20%)
    public int remainingTurns;   // �ʿ� ���ٸ� ���� ����

    public ActiveBuff(StatType stat, ModKind kind, float value, int turns = 0)
    {
        targetStat = stat;
        modKind = kind;
        this.value = value;
        remainingTurns = turns;
    }

}