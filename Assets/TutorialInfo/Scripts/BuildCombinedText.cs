
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class BuildCombinedText : MonoBehaviour
{
    [Header("BuffData")]

    [SerializeField] private BuffData source;          // 공통 템플릿
    [SerializeField] private BuffData[] sources;       // 슬롯별 템플릿(선택)

    [Header("Spawn Settings")]
    [SerializeField] private TextMeshProUGUI textPrefab;   // 만들 텍스트 프리팹(TMP_Text)
    [SerializeField] private RectTransform spawnParent;    // 생성될 부모(패널/캔버스 밑)
    [SerializeField] private int spawnCount = 3;           // 생성할 텍스트 개수(3개)
    [SerializeField] private bool startHidden = true;   // 시작 시 숨기기

    [Header("When a text is clicked (optional)")]
    public UnityEvent<string> onSkillChosen;               // 클릭시 스킬 문자열 콜백

    private ActiveBuff[] insts = new ActiveBuff[4];        // inst1~4 저장
    private readonly List<GameObject> spawned = new();     // 생성해둔 텍스트들


    void Start()
    {
        if (startHidden && spawnParent) spawnParent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            GenerateMany(Mathf.Max(spawnCount, 1)); // insts 채우기
            SpawnTexts();                             // 텍스트 생성 & 내용 채우기
        }
    }

    // --- 텍스트들 동적 생성 ---

    void SpawnTexts()
    {
        if (!textPrefab || !spawnParent)
        {

            Debug.LogWarning("BuffPreviewUI: textPrefab 또는 spawnParent가 비었습니다.");
            return;
        }

 
        foreach (var go in spawned) if (go) Destroy(go);
        spawned.Clear();

        spawnParent.gameObject.SetActive(true);

        for (int i = 0; i < spawnCount; i++)
        {

            int idx = i; // insts[idx]를 표시
            var t = Instantiate(textPrefab, spawnParent);
            t.raycastTarget = true;                   // 클릭 받기
            t.text = GetCombinedText(idx);         // 한 줄 내용

            // 클릭 핸들러 붙이기

            var item = t.gameObject.AddComponent<BuffTextItem>();
            item.Init(this, idx);

            spawned.Add(t.gameObject);
        }
    }


    // --- 텍스트 클릭 시: 정보 반환 + 텍스트 제거 ---
    public void OnItemClicked(GameObject item, int index)
    {
        string info = GetCombinedText(index); // "Stat / Kind / +값 / nT"
        onSkillChosen?.Invoke(info);            // 필요하면 인스펙터에서 이벤트로 받기

        Debug.Log($"[Selected] {info}");
        if (item) Destroy(item);
    }


    string FormatBuff(ActiveBuff b)
    {
        if (b == null) return "";
        string valueStr = (b.modKind == ModKind.Add)
            ? $"+{b.value}"
            : $"+{(b.value <= 1f ? b.value * 100f : b.value)}%";
        return $"{b.targetStat} / {b.modKind} / {valueStr} / {b.remainingTurns}T";
    }

    string GetCombinedText(params int[] indices)
    {
        if (indices == null || indices.Length == 0)

            indices = new int[] { 0, 1, 2, 3 }; // 기본: 전부


        var parts = new List<string>();
        foreach (var idx in indices)
        {
            if (idx < 0 || idx >= insts.Length) continue;
            var b = insts[idx];
            if (b == null) continue;
            parts.Add($"{idx + 1}) {FormatBuff(b)}");
        }
        return string.Join("\n", parts);
    }

    void GenerateMany(int n)
    {
        if (insts == null || insts.Length < 4) insts = new ActiveBuff[4];
        for (int i = 0; i < n && i < insts.Length; i++)
        {
            var src = (sources != null && i < sources.Length && sources[i] != null)
                        ? sources[i]
                        : source;
            insts[i] = src ? src.CreateInstance() : null;
        }
    }
}

