using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BuffTextItem : MonoBehaviour, IPointerClickHandler
{
    private BuildCombinedText owner;
    private int index;

    public void Init(BuildCombinedText owner, int index)
    {
        this.owner = owner;
        this.index = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (owner != null) owner.OnItemClicked(gameObject, index);
    }
}

