using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance;

    public DiceRoller diceRoller;
    public SkillManager skillManager;

    public enum GameState
    {
        SkillReset,
        SkillSelection,
        DiceRolling,
        ResultAndState,
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
                ChangeState(GameState.SkillSelection);
                break;
            case GameState.SkillSelection:
                Debug.Log("스킬 선택 상태: 스킬을 교체하고 주사위 버튼을 누르세요.");
                break;
            case GameState.DiceRolling:
                Debug.Log("주사위 굴리기 상태: 주사위가 굴러갑니다.");
                diceRoller.RollAndDisplayResult(); // 주사위 굴리기 함수 호출
                break;
            case GameState.ResultAndState:
                EndAction1AndLoadNextScene();
                break;
        }
    }

    // DiceButton에 연결할 함수
    public void OnDiceButtonPushed()
    {
        // 현재 상태가 SkillSelection일 때만 상태를 변경
        if (currentState == GameState.SkillSelection)
        {
            ChangeState(GameState.DiceRolling);
        }
    }

    private void EndAction1AndLoadNextScene()
    {
        Debug.Log("행동1 종료. 행동2 씬으로 전환합니다.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}