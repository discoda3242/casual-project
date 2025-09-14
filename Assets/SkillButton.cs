using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public int skillIndex;
    private SkillManager skillManager;

    void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();
    }

    public void OnClick()
    {
        if (skillManager != null)
        {
            // SkillManager에게 스킬 교체를 요청
            skillManager.ReplaceSkill(skillIndex);

            // 상태를 변경할 필요가 없으므로 GameStateMachine을 호출하지 않습니다.
        }
    }
}