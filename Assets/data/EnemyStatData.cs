using UnityEngine;
using System.IO;

[System.Serializable]
public class EnemyStat
{
    public string name;
    public float maxHP;
    public float attack;
    public float defense;
}

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "Game/Enemy Stat Data")]
public class EnemyStatData : ScriptableObject
{
    public EnemyStat[] enemies;

#if UNITY_EDITOR
    [ContextMenu("Import From CSV")]
    public void ImportFromCSV()
    {
        TextAsset csv = Resources.Load<TextAsset>("EnemyStats"); // Resources/EnemyStats.csv
        string[] lines = csv.text.Split('\n');

        enemies = new EnemyStat[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cols = lines[i].Split(',');

            EnemyStat e = new EnemyStat();
            e.name = cols[0];
            e.maxHP = float.Parse(cols[1]);
            e.attack = float.Parse(cols[2]);
            e.defense = float.Parse(cols[3]);

            enemies[i - 1] = e;
        }

        Debug.Log("CSV Import 완료!");
    }
#endif
}