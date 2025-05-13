using UnityEngine;
using System.Collections;
public class EtherealSlime : Card
{
    public override void Start()
    {
        base.Start();
    }
    public override IEnumerator ApplyBuffs()
    {
        if (parentRow == null) yield break;
        int index = parentRow.cards.IndexOf(this);
        if (index >= 0 && index + 1 < parentRow.cards.Count)
        {
            Card rightCard = parentRow.cards[index + 1];
            if (rightCard == null) yield break;
            Vector3 spawnPosition = rightCard.transform.position;
            GameObject fx = Instantiate(buffEffect, spawnPosition, Quaternion.identity);
            Destroy(fx, 0.9f);
            // Удаляем старую карту
            parentRow.cards.RemoveAt(index + 1);
            Destroy(rightCard.gameObject);
            yield return new WaitForEndOfFrame(); // Ждём удаления
            // Спавним копию себя через SpawnCardFromPrefab
            var card = parentRow.SpawnCardFromPrefab(gameObject, index + 1);
            card.health = health;
            card.attack = attack;
            Debug.Log($"{name} превратил карту справа в свою копию");
        }
        yield return null;
    }
}