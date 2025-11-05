using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "New Buff Database", menuName = "Game/Buff Database")]
public class BuffDatabase : ScriptableObject
{
    [Header("등록된 모든 버프들")]
    public List<BuffData> buffs = new List<BuffData>();
}

#if UNITY_EDITOR


[CustomEditor(typeof(BuffDatabase))]
public class BuffDatabaseEditor : Editor
{
    SerializedProperty buffsProp;
    Vector2 scroll;
    GUIStyle head, cellMini;

    void OnEnable()
    {
        buffsProp = serializedObject.FindProperty("buffs");

        head = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 12 };
        cellMini = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(2);
        EditorGUILayout.LabelField("🧩 Buff Database Manager", head);
        EditorGUILayout.Space(4);

        // Toolbar
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("＋ 슬롯 추가", GUILayout.Height(22)))
                buffsProp.arraySize++;

            if (GUILayout.Button("⚙ 새 BuffData 에셋 만들기", GUILayout.Height(22)))
                CreateNewBuffAsset();

            if (GUILayout.Button("↕ 정렬(ID)", GUILayout.Height(22)))
                SortById();

            if (GUILayout.Button("🧹 Null 제거", GUILayout.Height(22)))
                RemoveNulls();
        }

        EditorGUILayout.Space(4);

        if (buffsProp.arraySize == 0)
            EditorGUILayout.HelpBox("현재 등록된 BuffData가 없습니다. 위의 버튼으로 추가하거나, Project의 BuffData 에셋을 아래로 드래그하세요.", MessageType.Info);

        // Drag & Drop 영역 (여러 개 한 번에 추가)
        DrawDropArea();

        // Header Row
        if (buffsProp.arraySize > 0)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Obj", GUILayout.Width(40));
                GUILayout.Label("ID", GUILayout.Width(36));
                GUILayout.Label("이름", GUILayout.Width(140));
                GUILayout.Label("스탯", GUILayout.Width(70));
                GUILayout.Label("방식", GUILayout.Width(56));
                GUILayout.Label("효과(최~최)", GUILayout.Width(100));
                GUILayout.Label("지속(최~최)", GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                GUILayout.Label(" ", GUILayout.Width(130));
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        // Rows
        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.MinHeight(180));
        for (int i = 0; i < buffsProp.arraySize; i++)
        {
            SerializedProperty elem = buffsProp.GetArrayElementAtIndex(i);

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                elem.objectReferenceValue = EditorGUILayout.ObjectField(elem.objectReferenceValue, typeof(BuffData), false,
                    GUILayout.Width(40));

                if (elem.objectReferenceValue is BuffData data)
                {
                    GUILayout.Label(data.id.ToString(), GUILayout.Width(36));
                    GUILayout.Label(Trunc(data.buffName, 18), cellMini, GUILayout.Width(140));
                    GUILayout.Label(data.targetStat.ToString(), cellMini, GUILayout.Width(70));
                    GUILayout.Label(data.modKind.ToString(), cellMini, GUILayout.Width(56));
                    GUILayout.Label($"{data.minEffect}~{data.maxEffect}", cellMini, GUILayout.Width(100));
                    GUILayout.Label($"{data.minDuration}~{data.maxDuration}", cellMini, GUILayout.Width(100));
                }
                else
                {
                    GUILayout.Label("-", GUILayout.Width(36));
                    GUILayout.Label("(BuffData 없음)", cellMini, GUILayout.Width(140));
                    GUILayout.Label("-", GUILayout.Width(70));
                    GUILayout.Label("-", GUILayout.Width(56));
                    GUILayout.Label("-", GUILayout.Width(100));
                    GUILayout.Label("-", GUILayout.Width(100));
                }

                GUILayout.FlexibleSpace();

                using (new EditorGUILayout.HorizontalScope(GUILayout.Width(130)))
                {
                    GUI.enabled = elem.objectReferenceValue != null;
                    if (GUILayout.Button("열기", GUILayout.Width(40)))
                        Selection.activeObject = elem.objectReferenceValue;

                    GUI.enabled = true;
                    if (GUILayout.Button("▲", GUILayout.Width(25)) && i > 0)
                        Swap(i, i - 1);
                    if (GUILayout.Button("▼", GUILayout.Width(25)) && i < buffsProp.arraySize - 1)
                        Swap(i, i + 1);

                    if (GUILayout.Button("X", GUILayout.Width(25)))
                        DeleteAt(i);
                }
            }
        }
        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedProperties();
    }

    // ---------- helpers ----------
    void Swap(int a, int b)
    {
        buffsProp.serializedObject.Update();
        var tmp = buffsProp.GetArrayElementAtIndex(a).objectReferenceValue;
        buffsProp.GetArrayElementAtIndex(a).objectReferenceValue = buffsProp.GetArrayElementAtIndex(b).objectReferenceValue;
        buffsProp.GetArrayElementAtIndex(b).objectReferenceValue = tmp;
        buffsProp.serializedObject.ApplyModifiedProperties();
    }

    void DeleteAt(int i)
    {
        buffsProp.DeleteArrayElementAtIndex(i);
        // 두 번 호출해야 완전히 제거되는 경우가 있어 한 번 더.
        if (i < buffsProp.arraySize && buffsProp.GetArrayElementAtIndex(i).objectReferenceValue == null)
            buffsProp.DeleteArrayElementAtIndex(i);
    }

    void RemoveNulls()
    {
        for (int i = buffsProp.arraySize - 1; i >= 0; i--)
            if (buffsProp.GetArrayElementAtIndex(i).objectReferenceValue == null)
                DeleteAt(i);
    }

    void SortById()
    {
        // 간단 정렬: null은 뒤로
        var list = new List<BuffData>();
        for (int i = 0; i < buffsProp.arraySize; i++)
        {
            var r = buffsProp.GetArrayElementAtIndex(i).objectReferenceValue as BuffData;
            if (r != null) list.Add(r);
        }
        list.Sort((x, y) => x.id.CompareTo(y.id));

        buffsProp.arraySize = list.Count;
        for (int i = 0; i < list.Count; i++)
            buffsProp.GetArrayElementAtIndex(i).objectReferenceValue = list[i];
    }

    void CreateNewBuffAsset()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create BuffData", "New Buff", "asset", "파일 이름을 정하세요");
        if (string.IsNullOrEmpty(path)) return;

        var obj = ScriptableObject.CreateInstance<BuffData>();
        AssetDatabase.CreateAsset(obj, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        // 리스트에 추가
        buffsProp.arraySize++;
        buffsProp.GetArrayElementAtIndex(buffsProp.arraySize - 1).objectReferenceValue = obj;
    }

    void DrawDropArea()
    {
        var rect = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));
        GUI.Box(rect, "여기로 BuffData 에셋을 드래그하세요 (여러 개 가능)", EditorStyles.helpBox);

        var evt = Event.current;
        if (!rect.Contains(evt.mousePosition)) return;

        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (var o in DragAndDrop.objectReferences)
                {
                    if (o is BuffData bd)
                    {
                        buffsProp.arraySize++;
                        buffsProp.GetArrayElementAtIndex(buffsProp.arraySize - 1).objectReferenceValue = bd;
                    }
                }
            }
            Event.current.Use();
        }
    }

    string Trunc(string s, int len) => string.IsNullOrEmpty(s) ? "" : (s.Length <= len ? s : s.Substring(0, len - 1) + "…");
}
#endif
