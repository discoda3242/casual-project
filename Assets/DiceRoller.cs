using UnityEngine;
using System.Collections;
using TMPro; // TextMeshPro를 사용하려면 추가해야 합니다.

public class DiceRoller : MonoBehaviour
{
    // 인스펙터에 연결할 변수들
    public TMP_Text diceResultText;
    public int diceSides = 6;

    // 주사위 굴리기 애니메이션 시간
    public float rollTime = 1f;
    private bool isRolling = false;

    // 게임매니저가 호출하는 함수
    public void RollAndDisplayResult()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDiceCoroutine());
        }
    }

    IEnumerator RollDiceCoroutine()
    {
        isRolling = true;
        float timer = 0f;

        // 주사위가 굴러가는 듯한 시각적 효과
        while (timer < rollTime)
        {
            int roll = Random.Range(1, diceSides + 1);
            diceResultText.text = $"Cost: {roll}";
            timer += Time.deltaTime;
            yield return null;
        }

        // 최종 주사위 값 결정
        int finalRoll = Random.Range(1, diceSides + 1);
        diceResultText.text = $"Cost: {finalRoll}";

        // 최종 주사위 값을 GameData에 저장
        if (GameData.Instance != null)
        {
            GameData.Instance.diceValueFromAction1 = finalRoll;
            Debug.Log($"주사위 결과 {finalRoll}이(가) GameData에 저장되었습니다.");
        }

        // 주사위 굴리기가 끝났음을 GameStateMachine에 알림
        if (GameStateMachine.Instance != null)
        {
            GameStateMachine.Instance.OnDiceRollCompleted();
        }

        isRolling = false;
    }
}