using UnityEngine;
using System.Collections;

public class ImposibleSlime : Card
{
    public override void Start()
    {
        base.Start();
    }

    public override IEnumerator ApplyBuffs()
    {
        if (parentRow == null)
        {
            Debug.LogWarning("ImposibleSlime: parentRow is null!");
            yield break;
        }

        if (parentRow.cards.Count < parentRow.columns)
        {
            yield return new WaitForSeconds(0.3f);

            Vector3 spawnPosition = parentRow.transform.position;

            if (parentRow.cards.Count > 0 && parentRow.cards[parentRow.cards.Count - 1] != null)
            {
                Transform lastCard = parentRow.cards[parentRow.cards.Count - 1].transform;
                spawnPosition = lastCard.position + new Vector3(1.5f, 0, 0);
            }

            GameObject clone = Instantiate(gameObject, spawnPosition, Quaternion.identity, parentRow.transform);
            GameObject fx = Instantiate(buffEffect, spawnPosition, Quaternion.identity);
            Destroy(fx, 0.9f);

            var newCard = clone.GetComponent<ImposibleSlime>();
            if (newCard != null)
            {
                parentRow.cards.Add(newCard);
                newCard.parentRow = parentRow;
                Debug.Log("ImposibleSlime: Клон создан.");
            }
            else
            {
                Debug.LogWarning("ImposibleSlime: Клон не содержит компонент ImposibleSlime.");
            }
        }
    }
}
