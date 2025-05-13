using UnityEngine;
using System.Collections.Generic;

public class MagicSlime : Card
{
    public override void Start()
    {
        base.Start();
        battleManager.onEnd += Sales;
    }

    private void OnDestroy()
    {
        if (battleManager != null)
            battleManager.onEnd -= Sales;
    }

    void Sales()
    {
        if (this == null || gameObject == null) return;

        List<Card> allCards = FindFirstObjectByType<Deck>().deckCards;
        foreach (Card card in allCards)
        {
            if (card != null && card.isSpell)
            {
                card.mana = Mathf.Max(0, Mathf.FloorToInt(card.mana / 2f));
            }
        }
    }
}
