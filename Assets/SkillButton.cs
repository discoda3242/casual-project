using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public int skillIndex;
    public BuffData buffData;

    void Start()
    {
    }

    public void OnClick()
    {
        ActiveBuff data = buffData.CreateInstance(); // 인스턴스 메서드로 호출
        DeckManager deckManager = FindObjectOfType<DeckManager>();
        if (deckManager != null)
        {
            deckManager.AddCardToDeck(data);
        }
        else
        {
            Debug.LogError("DeckManager 인스턴스를 찾을 수 없습니다.");
        }


    }
}