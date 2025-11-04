using UnityEngine;

public class Action2Manager : MonoBehaviour
{
    // 이 스크립트가 행동2의 모든 로직을 관리
    public void StartAction2()
    {
        Debug.Log("Action2Manager: 행동2 로직 시작");
        // 여기에 행동2의 모든 로직을 구현합니다.
        // 예를 들어, 플레이어가 스킬을 선택하고 턴을 넘기는 등의 로직을 처리합니다.

        // 행동2가 완전히 끝났을 때만 GameManager에게 알립니다.
        // GameStateMachine.Instance.EndAction2();
    }
}