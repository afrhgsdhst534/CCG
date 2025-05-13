using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LostSlime : Card
{
    public GameObject monsterSlimePrefab;
    public override IEnumerator ApplyBuffs()
    {
        var neighbors = GetNeighbors();
        int index = parentRow.cards.IndexOf(this);
        List<Card> toRemove = new List<Card>();
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
                toRemove.Add(neighbor);
        }
        toRemove.Add(this); // ������ � ������� �����
        StartCoroutine(ReplaceWithMonster(index, toRemove));
        yield break; // �� ���������� ApplyBuffs
    }
    private IEnumerator ReplaceWithMonster(int originalIndex, List<Card> toRemove)
    {
        yield return new WaitForEndOfFrame();
        foreach (var card in toRemove)
        {
            if (card != null && card.gameObject != null)
            {
                parentRow.cards.Remove(card);
                Destroy(card.gameObject);
            }
        }
        if (monsterSlimePrefab != null)
        {
            int siblingIndex = transform.GetSiblingIndex();
            parentRow.cards.Remove(this);

            var monster = Instantiate(monsterSlimePrefab, parentRow.transform);
            monster.transform.SetSiblingIndex(siblingIndex);
            var monsterCard = monster.GetComponent<Card>();
            monsterCard.parentRow = parentRow;
            parentRow.cards.Insert(siblingIndex, monsterCard);
            parentRow.Sync();

            Destroy(gameObject); // ���������� ���� � ����� �����
        }
    }
}