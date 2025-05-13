using System.Collections;
using UnityEngine;
public class SlimesPunch : Card
{
    public override IEnumerator UseAbilities()
    {
        Row enemyRow = battleManager.playerRow;
        if (enemyRow.cards.Count == 0)
            yield break;
        Card strongest = null;
        int maxAttack = int.MinValue;
        foreach (var enemyCard in enemyRow.cards)
        {
            if (enemyCard.attack > maxAttack)
            {
                maxAttack = enemyCard.attack;
                strongest = enemyCard;
            }
        }
        if (strongest == null || strongest == this)
            yield break;
        if (buffEffect != null)
        {
            GameObject fx = Instantiate(buffEffect, transform.position, Quaternion.identity);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y, 0);
            Destroy(fx, 0.9f);
        }
        yield return new WaitForSeconds(0.3f);
        int stolenAmount = strongest.attack / 2;
        strongest.attack -= stolenAmount;
        this.attack += stolenAmount;
        Debug.Log($"{name} украл {stolenAmount} атаки у {strongest.name}");
    }
}