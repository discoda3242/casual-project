using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private int currentCost = 0;
    public TMP_Text costText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ?? 이 함수가 GameData에서 코스트 값을 가져와 초기화합니다.
    public void InitializeCost()
    {
        if (GameData.Instance != null)
        {
            currentCost = GameData.Instance.diceValueFromAction1;
            Debug.Log($"행동1에서 가져온 초기 코스트: {currentCost}");
            UpdateCostUI();
        }
    }

    public void AddCost(int amount)
    {
        currentCost += amount;
        UpdateCostUI();
        Debug.Log($"코스트 {amount} 획득. 현재 코스트: {currentCost}");
    }

    public bool HasSufficientCost(int requiredCost)
    {
        return currentCost >= requiredCost;
    }

    public void ConsumeCost(int amount)
    {
        currentCost -= amount;
        UpdateCostUI();
        Debug.Log($"코스트 {amount} 소모. 남은 코스트: {currentCost}");
    }

    public void UpdateCostUI()
    {
        if (costText != null)
        {
            // "코스트:"를 "Cost:"로 변경
            costText.text = $"Cost: {currentCost}";
        }
    }

    public int GetCurrentCost()
    {
        return currentCost;
    }
}