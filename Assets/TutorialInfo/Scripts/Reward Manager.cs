using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { Attack, Defense, Speed, HP }
public enum ModKind { Add, Mult }  // Add: 고정 수치, Mult: 비율

[CreateAssetMenu(fileName = "New Buff", menuName = "Game/Buff Data")]
public class BuffData : ScriptableObject
{
    [Header("기본 정보")]
    public int id;                // 버프 고유 ID
    public string buffName;       // 버프 이름 (UI 표시용)
    public string description;    // 설명 (툴팁 등)

    [Header("효과 대상")]
    public StatType targetStat;   // 어떤 스탯에 적용되는지
    public ModKind modKind;       // 고정 추가인지, 배율 곱연산인지

    [Header("효과 범위")]
    public int minDuration;       // 지속턴 최소값
    public int maxDuration;       // 지속턴 최대값
    public int minEffect;         // 효과 최소값
    public int maxEffect;         // 효과 최대값

    [Header("연출")]
    public Sprite icon;           // 아이콘 이미지
    public Color buffColor;       // UI 강조색 (선택사항)

    // 실제 적용할 때 랜덤 굴려서 ActiveBuff 반환
    public ActiveBuff CreateInstance()
    {
        int duration = Random.Range(minDuration, maxDuration + 1);
        int effectValue = Random.Range(minEffect, maxEffect + 1);

        return new ActiveBuff(targetStat, modKind, effectValue, duration);
    }
}