using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffPreviewUI : MonoBehaviour
{
    [Header("공통 BuffData (아래 sources가 비어있으면 이걸로 4개 생성)")]
    [SerializeField] private BuffData source;

    [Header("슬롯별 BuffData (선택, 크기 4로 두고 각자 다른 템플릿 쓰고 싶을 때)")]
    [SerializeField] private BuffData[] sources; // 비워두면 source 사용

    [Header("출력 TMP 텍스트들")]
    [SerializeField] private TMP_Text Text1;
    [SerializeField] private TMP_Text Text2;
    [SerializeField] private TMP_Text Text3;
    [SerializeField] private TMP_Text Text4;

    // inst1~inst4를 담아둘 곳
    private ActiveBuff[] insts = new ActiveBuff[4];
    private int current = 0; // 지금 화면에 보여줄 인덱스(0~3)

    private void Update()
    {
        // 스페이스: inst1~4 전부 새로 굴리고, current(기본 0번) 보여주기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateMany(4);
        }
        ShowCombined(Text1, 1);
        ShowCombined(Text2, 2);
        ShowCombined(Text3, 3);

        // 숫자 1~4로 표시 대상을 전환

    }


    string FormatBuff(ActiveBuff b)
    {
        if (b == null) return "";
        string valueStr = (b.modKind == ModKind.Add)
            ? $"+{b.value}"
            : $"+{(b.value <= 1f ? b.value * 100f : b.value)}%";
        return $"{b.targetStat} / {b.modKind} / {valueStr} / {b.remainingTurns}T";
    }

    // 한 줄(또는 여러 줄) 텍스트로 합쳐 반환
    string BuildCombinedText(params int[] indices)
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

        // 줄바꿈으로 합치기(한 줄로만 원하면 " | " 로 바꾸세요)
        return string.Join("\n", parts);
    }

    // TMP_Text에 바로 써주는 편의 함수
    void ShowCombined(TMP_Text label, params int[] indices)
    {
        if (!label) return;
        label.text = BuildCombinedText(indices);
    }
    private void GenerateMany(int n)
    {
        for (int i = 0; i < n && i < insts.Length; i++)
        {
            var src = (sources != null && i < sources.Length && sources[i] != null)
                        ? sources[i]
                        : source;

            insts[i] = src ? src.CreateInstance() : null;
        }
    }

    // 필요하면 외부에서 inst1~4 꺼내 쓰기
    public ActiveBuff GetInst(int i) =>
        (i >= 0 && i < insts.Length) ? insts[i] : null;
}