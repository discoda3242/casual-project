using UnityEngine;

public class UIManager : MonoBehaviour
{
    public DeckManager deckManager;

    void Start()
    {
        // 덱 꽉 참 이벤트 구독
        deckManager.OnDeckFull += ShowDeckFullMessage;
    }

    void ShowDeckFullMessage()
    {
        Debug.Log("UI 메시지: 덱이 꽉 찼습니다!");
        // 여기서 팝업 띄우거나 경고음 재생 가능
    }
}