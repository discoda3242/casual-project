using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour
{
    [SerializeField] private EnemyStageSwapper target; // 데미지 받을 대상
    [SerializeField] private Button button;            // 이 오브젝트의 Button
    [SerializeField] private float damage = 10f;       // 한 번 누를 때 줄 데미지

    private void Reset()
    {
        // 에디터에서 자동 할당 편의
        button = GetComponent<Button>();
        if (target == null) target = FindObjectOfType<EnemyStageSwapper>();
    }

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (button != null) button.onClick.AddListener(DoDamage);
    }

    private void OnDestroy()
    {
        if (button != null) button.onClick.RemoveListener(DoDamage);
    }

    public void DoDamage()
    {
        if (target == null)
        {
            Debug.LogWarning("[DamageButton] target(EnemyStageSwapper)가 비었습니다.");
            return;
        }
        target.TakeDamage(damage);
    }
}
