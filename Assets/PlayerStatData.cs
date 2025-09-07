//플레이어 스탯 관리 스크립터블 오브젝트
using UnityEngine;

[CreateAssetMenu(fileName = "StatData", menuName = "Game/Stat Data")]
public class StatData : ScriptableObject
{
    [Header("Base Stats")]
    public float baseMaxHP = 100f;
    public float baseAttack = 10f;
    public float baseDefense = 5f;
}
