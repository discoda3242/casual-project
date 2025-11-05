using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance;
    public DiceRoller diceRoller;

    // ⛔ 기존: public SkillManager skillManager;
    // ✅ 교체:
    public DeckManager deckManager;

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
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    void Start()
    {
        ChangeState(GameState.SkillReset);
    }

    // (선택) 버튼에서 상태 분기하고 싶으면 사용
    public GameState GetState() => currentState;

    public void OnDiceButtonPushed()
    {
        if (currentState == GameState.SkillSelection)
            ChangeState(GameState.DiceRolling);
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
                // ⛔ 기존: if (skillManager != null) skillManager.ResetSkills();
                // ✅ 교체:
              
                   

                if (action1UI != null) action1UI.SetActive(true);
                if (action2UI != null) action2UI.SetActive(false);
                ChangeState(GameState.SkillSelection);
                break;

            case GameState.SkillSelection:
                Debug.Log("스킬 선택 상태: 덱에 카드 추가/교체 후 주사위 버튼을 누르세요.");
                break;

            case GameState.DiceRolling:
                diceRoller.RollAndDisplayResult();
                break;

            case GameState.ResultAndState:
                EndAction1AndStartAction2();
                break;

            case GameState.Action2_Active:
                if (action1UI != null) action1UI.SetActive(false);
                if (action2UI != null) action2UI.SetActive(true);

                if (playerManager != null) playerManager.InitializeCost();
                if (action2UIUpdater != null) action2UIUpdater.UpdateCostUI();
                Debug.Log("행동2 활성화: 스킬(카드) 사용 단계.");
                break;
        }
    }

    private void EndAction1AndStartAction2()
    {
        Debug.Log("행동1 종료. 행동2로 전환.");
        ChangeState(GameState.Action2_Active);
    }
}
