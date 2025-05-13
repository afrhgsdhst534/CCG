using System.Collections;
using UnityEngine;
public class UniversalSlime : Card
{
    public override IEnumerator ApplyBuffs()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var card in parentRow.cards)
        {
            if (card == this) continue; // не бафаем самого себя
            card.health += this.health;
            card.attack += this.attack;
            if (card.attack < 0) card.attack = 0; // защита от отрицательной атаки
            if (buffEffect)
            {
                var fx = Instantiate(buffEffect, card.transform.position, Quaternion.identity);
                fx.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, 0);
                Destroy(fx, 0.9f);
            }
        }
        Debug.Log("UniversalSlime: дал здоровье, забрал силу.");
    }
}