using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffDatabase", menuName = "Game/Buff Database")]
public class BuffDatabase : ScriptableObject
{
    [Tooltip("여기에 BuffData 에셋들을 드래그&드롭 하세요.")]
    public List<BuffData> entries = new List<BuffData>();

    public int Count => entries?.Count ?? 0;

    public BuffData GetById(int id)
    {
        if (entries == null) return null;
        return entries.Find(b => b != null && b.id == id);
    }

    public BuffData GetRandomData()
    {
        if (entries == null || entries.Count == 0) return null;
        return entries[Random.Range(0, entries.Count)];
    }

    /// <summary>
    /// 무작위 BuffData를 하나 골라 즉시 ActiveBuff 인스턴스를 만들어 반환.
    /// </summary>
    public ActiveBuff CreateRandomInstance()
    {
        var data = GetRandomData();
        return data ? data.CreateInstance() : null;
    }
}