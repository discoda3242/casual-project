using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public int skillIndex;

    public void OnClick()
    {
        // GameManager에 연결된 SkillManager를 찾아서 함수 호출
        SkillManager skillManager = FindObjectOfType<SkillManager>();
        if (skillManager != null)
        {
            // SkillManager에게 이 버튼의 인덱스를 알려줘서 스킬을 교체
            skillManager.ReplaceSkill(skillIndex);
        }

        // 이제 스킬 버튼을 눌러도 상태는 SkillSelection에 머무르게 됩니다.
    }
}