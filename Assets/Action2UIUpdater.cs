using UnityEngine;
using TMPro;

public class Action2UIUpdater : MonoBehaviour
{
    // 행동2 캔버스에 있는 코스트 텍스트를 인스펙터에서 연결
    public TMP_Text costText;

    // 이 함수는 행동2 캔버스가 활성화될 때 자동으로 호출됩니다.
    void OnEnable()
    {
        UpdateCostUI();
    }

    // PlayerManager로부터 코스트를 가져와 UI를 업데이트합니다.
    public void UpdateCostUI()
    {
        if (PlayerManager.Instance != null && costText != null)
        {
            int currentCost = PlayerManager.Instance.GetCurrentCost();
            costText.text = $"cost: {currentCost}";
        }
    }
}