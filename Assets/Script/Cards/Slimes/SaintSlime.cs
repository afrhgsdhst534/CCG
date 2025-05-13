using UnityEngine;
using System.Collections;

public class SaintSlime : Card
{
    public int maxResurrects = 3;

    public override IEnumerator ApplyBuffs()
    {
        var neighbors = GetNeighbors();
        maxResurrects--;
        foreach (var neighbor in neighbors)
        {
            if (neighbor == null) continue;

            // Подписываемся на смерть соседа, проверяя на null
            if (neighbor.onDeath != null)
            {
                neighbor.onDeath -= OnNeighborDeath; // Убираем старую подписку, если она есть
            }
            neighbor.onDeath += OnNeighborDeath;
        }

        yield break;
    }

    private void OnNeighborDeath(Card deadNeighbor)
    {
        try
        {
            // Проверяем, существует ли deadNeighbor, если объект уничтожен — выходим
            if (deadNeighbor == null)
            {
                return;
            }

            // Проверка, был ли уничтожен объект
            if (deadNeighbor.gameObject == null)
            {
                return;
            }

            if (maxResurrects <= 0) return;

            int maxStat = Mathf.Max(deadNeighbor.attack, deadNeighbor.health);

            // Клонируем погибшего соседа
            GameObject clone = Instantiate(deadNeighbor.gameObject);
            clone.transform.SetParent(parentRow.transform, false);

            // Вставляем в нужную позицию в списке и иерархии
            int index = deadNeighbor.transform.GetSiblingIndex();
            clone.transform.SetSiblingIndex(index);

            Card resurrected = clone.GetComponent<Card>();
            resurrected.attack = maxStat;
            resurrected.health = maxStat;
            resurrected.parentRow = parentRow;

            parentRow.cards.Insert(index, resurrected);

            // Эффект воскрешения (опционально)
            GameObject fx = Instantiate(buffEffect, resurrected.transform.position, Quaternion.identity);
            fx.transform.position = new Vector3(resurrected.transform.position.x, resurrected.transform.position.y, 0);

            Destroy(fx, 0.9f);

        }
        catch 
        {

        }

    }
}
