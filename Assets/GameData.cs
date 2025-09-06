using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int diceValueFromAction1; // 행동1에서 저장할 주사위 값

    private void Awake()
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
}