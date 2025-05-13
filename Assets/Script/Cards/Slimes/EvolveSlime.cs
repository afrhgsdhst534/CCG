using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvolveSlime : Card
{
    public List<Card> stolenCards = new List<Card>();
    private const int maxStolen = 3;

    public override void Start()
    {
        base.Start();
        battleManager.onEnd += Evolve;
    }

    private void OnDestroy()
    {
        if (battleManager != null)
            battleManager.onEnd -= Evolve;
    }

    private void Evolve()
    {
        try
        {
            if (this == null || gameObject == null) return;

            TryStealAbility();
            StartCoroutine(ActivateStolenBuffs());
        }
        catch { }
    }

    private void TryStealAbility()
    {
        try
        {
            if (parentRow == null) return;

            int index = parentRow.cards.IndexOf(this);
            stolenCards.RemoveAll(card => card == null);

            if (index > 0)
            {
                Card left = parentRow.cards[index - 1];
                if (left != null && !stolenCards.Contains(left) && stolenCards.Count < maxStolen)
                {
                    stolenCards.Add(left);
                    Debug.Log($"{name} украл способность у {left.name}");
                }
            }
        }
        catch { }
    }

    private IEnumerator ActivateStolenBuffs()
    {
        foreach (var card in stolenCards)
        {
            if (card == null || card.gameObject == null) continue;

            yield return StartCoroutine(card.ApplyBuffs());
        }
    }
}
