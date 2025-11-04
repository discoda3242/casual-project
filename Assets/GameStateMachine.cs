using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance;
    public DiceRoller diceRoller;
    public SkillManager skillManager;
    public PlayerManager playerManager;
    public Action2UIUpdater action2UIUpdater;

    public GameObject action1UI;
    public GameObject action2UI;

    public enum GameState
    {
        SkillReset,
        SkillSelection,
        DiceRolling,
        ResultAndState,
        Action2_Active
    }

    private GameState currentState;

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

    void Start()
    {
        ChangeState(GameState.SkillReset);
    }

    public void OnDiceButtonPushed()
    {
        if (currentState == GameState.SkillSelection)
        {
            ChangeState(GameState.DiceRolling);
        }
    }

    // ?? 이 함수가 주사위 굴리기가 끝났을 때 호출됩니다.
    public void OnDiceRollCompleted()
    {
        ChangeState(GameState.ResultAndState);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.SkillReset:
                if (skillManager != null)
                {
                    skillManager.ResetSkills();
                }
                if (action1UI != null) action1UI.SetActive(true);
                if (action2UI != null) action2UI.SetActive(false);
                ChangeState(GameState.SkillSelection);
                break;

            case GameState.SkillSelection:
                Debug.Log("스킬 선택 상태: 스킬을 교체하고 주사위 버튼을 누르세요.");
                break;

            case GameState.DiceRolling:
                Debug.Log("주사위 굴리기 상태: 주사위가 굴러갑니다.");
                diceRoller.RollAndDisplayResult();
                break;

            case GameState.ResultAndState:
                EndAction1AndStartAction2();
                break;

            case GameState.Action2_Active:
                if (action1UI != null) action1UI.SetActive(false);
                if (action2UI != null) action2UI.SetActive(true);

                // ?? 여기에서 PlayerManager의 초기화 함수를 호출합니다.
                if (playerManager != null)
                {
                    playerManager.InitializeCost();
                }
                if (action2UIUpdater != null)
                {
                    action2UIUpdater.UpdateCostUI();
                }
                Debug.Log("행동2 활성화: 스킬 이미지를 클릭하세요.");
                break;
        }
    }

    private void EndAction1AndStartAction2()
    {
        Debug.Log("행동1 종료. 행동2 캔버스로 전환합니다.");
        ChangeState(GameState.Action2_Active);
    }
}