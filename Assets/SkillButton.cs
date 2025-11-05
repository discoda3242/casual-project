using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public int skillIndex;
    public BuffDatabase buffData;

    [Header("스프라이트(아이콘) 이미지 리스트")]
    public List<Image> imageList = new List<Image>();  // UI Image 오브젝트 4개

    [Header("텍스트 리스트 (TMP_Text)")]
    public List<TMP_Text> textList = new List<TMP_Text>(); // TMP_Text 오브젝트 4개

    public void OnMouseDown()
    {
        if (buffData == null)
        {
            Debug.LogWarning("buffData가 비어 있습니다.");
            return;
        }

        // 1️⃣ 스킬 인스턴스 생성
        Debug.Log("클릭성공.");
        ActiveBuff data = buffData.CreateRandomInstance();

        // 2️⃣ 덱 매니저에 추가 (남의 코드 그대로)
        DeckManager deckManager = FindObjectOfType<DeckManager>();
        if (deckManager != null)
        {
            deckManager.AddCardToDeck(data);
            for (int i = 0; i < deckManager.deckSize; i++)
            {
                imageList[i].sprite = deckManager.ShowDeck()[i].sp;
                textList[i].text = deckManager.ShowDeck()[i].targetStat.ToString();
            }
        }
        else
        {
            Debug.LogError("DeckManager 인스턴스를 찾을 수 없습니다.");
        }

    }
}
