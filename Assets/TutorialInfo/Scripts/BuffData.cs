using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { Attack, Defense, Speed, HP }
public enum ModKind { Add, Mult }  // Add: 고정 수치, Mult: 비율

[CreateAssetMenu(fileName = "New Buff", menuName = "Game/Buff Data")]
public class BuffData : ScriptableObject
{
    [Header("기본 정보")]
    public int id;
    public string buffName;
    public string description;

    [Header("효과 대상")]
    public StatType targetStat;
    public ModKind modKind;

    [Header("효과 범위")]
    public int minDuration;
    public int maxDuration;
    public int minEffect;
    public int maxEffect;

    [Header("연출")]
    public Sprite icon;
    public Color buffColor;

    // 실제 적용 시 랜덤으로 ActiveBuff 생성
    public ActiveBuff CreateInstance()
    {
        int duration = Random.Range(minDuration, maxDuration + 1);
        int effectValue = Random.Range(minEffect, maxEffect + 1);
        return new ActiveBuff(targetStat, modKind, effectValue, duration);
    }

    // 🔹 BuffData를 테이블 형식으로 출력하는 함수
   
}
