/*
using UnityEngine;
using System.IO;


[System.Serializable]
public class EnemyStat
{
    public string name;
    public float maxHP;
    public float attack;
    public float defense;
    public Sprite icon;
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
*/
// EnemyStatData.cs는 표
// EnemyStageSwapper가 표를 읽어 슬라임 오브젝트에 바로 덮어씌우는 역할
using UnityEngine;
using System.Globalization;

[System.Serializable]
public class EnemyStat
{
    public string name;
    public float maxHP;
    public float attack;
    public float defense;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "Game/Enemy Stat Data")]
public class EnemyStatData : ScriptableObject
{
    public EnemyStat[] enemies;

    // === 조회 편의 API ===
    public int StageCount => enemies?.Length ?? 0;

    public EnemyStat GetEntry(int stage)
    {
        if (enemies == null || enemies.Length == 0) return null;
        stage = Mathf.Clamp(stage, 0, enemies.Length - 1);
        return enemies[stage];
    }

    public float  GetMaxHP (int stage) => GetEntry(stage)?.maxHP  ?? 1f;
    public float  GetAttack(int stage) => GetEntry(stage)?.attack ?? 0f;
    public float  GetDefense(int stage)=> GetEntry(stage)?.defense?? 0f;
    public Sprite GetIcon  (int stage) => GetEntry(stage)?.icon;

#if UNITY_EDITOR
    [ContextMenu("Import From CSV")]
    public void ImportFromCSV()
    {
        // Resources/EnemyStats.csv (첫 줄은 헤더라고 가정)
        TextAsset csv = Resources.Load<TextAsset>("EnemyStats");
        if (csv == null)
        {
            Debug.LogError("Resources/EnemyStats.csv 를 찾을 수 없습니다.");
            return;
        }

        string[] lines = csv.text.Split('\n');
        var list = new System.Collections.Generic.List<EnemyStat>();

        for (int i = 1; i < lines.Length; i++) // 헤더 다음 줄부터
        {
            var line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = line.Split(',');
            if (cols.Length < 4)
            {
                Debug.LogWarning($"CSV {i}행: 열 개수가 부족합니다 (필수: name,maxHP,attack,defense).");
                continue;
            }

            var e = new EnemyStat();
            e.name    = cols[0].Trim();
            e.maxHP   = float.Parse(cols[1], CultureInfo.InvariantCulture);
            e.attack  = float.Parse(cols[2], CultureInfo.InvariantCulture);
            e.defense = float.Parse(cols[3], CultureInfo.InvariantCulture);

            // 5열(icon 경로)이 있으면 Resources에서 스프라이트 로드 (예: "Sprites/Slime")
            if (cols.Length > 4)
            {
                string iconPath = cols[4].Trim().Trim('"');
                if (!string.IsNullOrEmpty(iconPath))
                    e.icon = Resources.Load<Sprite>(iconPath);
            }

            list.Add(e);
        }

        enemies = list.ToArray();
        Debug.Log($"CSV Import 완료! {enemies.Length}개 로드");
    }
#endif
}
