using System.Collections.Generic;
using UnityEngine;
using System; // Action 쓰려면 필요

public class DeckManager : MonoBehaviour
{
    public int deckSize = 10;
    private static List<ActiveBuff> deck = new List<ActiveBuff>();

    // 덱이 꽉 찼을 때 발생하는 이벤트
    public event Action OnDeckFull;

    public ActiveBuff UseCard(int i)
    {
        ActiveBuff selected = deck[i];
        deck.RemoveAt(i);
        return selected;
    }

    public List<ActiveBuff> ShowDeck()
    {
        return deck;
    }

    public void AddCardToDeck(ActiveBuff buff)
    {
        // 아직 deck 리스트 크기가 deckSize보다 작으면 그냥 추가
        if (deck.Count < deckSize)
        {
            deck.Add(buff);
        }
        else
        {
            Debug.Log("덱이 꽉 찼습니다!");
            OnDeckFull?.Invoke(); // 이벤트 발생
        }
    }
}