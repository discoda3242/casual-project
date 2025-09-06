using System.Collections;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하거나, UnityEngine.UI;를 사용

public class DiceRoller : MonoBehaviour
{
    // 게임 오브젝트를 직접 연결하는 public 변수
    public TMPro.TextMeshProUGUI diceResultText;
    public int diceSides = 6;
    private bool isRolling = false;

    // 게임 매니저가 호출하는 함수
    public void RollAndDisplayResult()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDiceCoroutine());
        }
    }

    private IEnumerator RollDiceCoroutine()
    {
        isRolling = true;

        float rollDuration = 1.5f;
        float timer = 0f;

        // 주사위 굴러가는 시각적 효과
        while (timer < rollDuration)
        {
            int tempRoll = Random.Range(1, diceSides + 1);
            diceResultText.text = "cost: " + tempRoll.ToString();
            timer += Time.deltaTime;
            yield return null;
        }

        // 최종 주사위 값 결정 및 출력
        int finalRoll = Random.Range(1, diceSides + 1);
        diceResultText.text = "cost: " + finalRoll.ToString();

        isRolling = false;

        // 최종 주사위 값을 GameData에 저장
        if (GameData.Instance != null)
        {
            GameData.Instance.diceValueFromAction1 = finalRoll;
        }

        // 주사위 굴리기가 끝나면 다음 상태로 전환하도록 게임 매니저에 알림
        if (GameStateMachine.Instance != null)
        {
            GameStateMachine.Instance.OnDiceRollCompleted();
        }
    }
}