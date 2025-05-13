using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterSlime : Card
{
    public bool isBeingDestroyed = false;

    public override void Start()
    {
        base.Start();
        parentRow.Sync();
    }
    public IEnumerator AbsorbCards(List<Card> cardsToAbsorb)
    {
        foreach (var card in cardsToAbsorb)
        {
            if (card == null) continue;

            yield return StartCoroutine(card.ApplyBuffs());

            attack += card.attack;
            health += card.health;

            Debug.Log($"Монстр поглотил {card.name}, сила = {attack}, здоровье = {health}");

            // Отложенное уничтожение
            StartCoroutine(DestroyAfterFrame(card.gameObject));
        }

        GetComponent<CardsInsides>()?.UpdateUI();
    }

    private IEnumerator DestroyAfterFrame(GameObject obj)
    {
        yield return null; // подождать 1 кадр
        Destroy(obj);
    }
    public override IEnumerator ApplyBuffs()
    {
        StartCoroutine(AbsorbCards(GetNeighbors()));
        yield break; // Больше ничего не делает сам по себе
    }
}