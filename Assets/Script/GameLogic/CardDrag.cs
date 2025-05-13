using UnityEngine;
using System.Collections.Generic;
public class CardDrag : MonoBehaviour
{
    private DraggableCard currentDragging;
    public List<Card> allCards;
    public void StartDragging(DraggableCard card)
    {
        currentDragging = card;

        // Отключаем обновление layout
        card.originalParent.GetComponent<Row>().freezeLayout = true;
    }
    public void StopDragging()
    {
        if (currentDragging != null)
        {
            currentDragging.originalParent.GetComponent<Row>().freezeLayout = false;
            currentDragging = null;
        }
    }
    public DraggableCard GetClosestCard(DraggableCard source)
    {
        float minDist = float.MaxValue;
        DraggableCard closest = null;
        foreach (Transform child in source.originalParent)
        {
            DraggableCard other = child.GetComponent<DraggableCard>();
            if (other == null || other == source) continue;
            float dist = Vector3.Distance(source.transform.position, other.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = other;
            }
        }
        return closest;
    }
}