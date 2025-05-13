using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SlimesAura : Card
{
    public GameObject effect;
    protected override IEnumerator DoAttack()
    {
        yield break;
    }
    public override IEnumerator UseAbilities()
    {
        var first =battleManager.playerRow.cards[0];
        var second = battleManager.playerRow.cards[^1];
        if (this == null || first == null) yield break;
        GameObject fx = Instantiate(buffEffect, first.transform.position, Quaternion.identity);
        fx.transform.position = new(first.transform.position.x, first.transform.position.y, 0);
        Destroy(fx, 0.9f); // уничтожить после проигрывания
        first.health += first.attack;
        yield return new WaitForSeconds(0.5f);
        GameObject fx1 = Instantiate(effect, second.transform.position, Quaternion.identity);
        fx1.transform.position = new(second.transform.position.x, second.transform.position.y, 0);
        Destroy(fx1, 0.9f); // уничтожить после проигрывания
        second.attack += second.health;
        yield return new WaitForSeconds(0.3f);
        parentRow.cards.Remove(this);
        Destroy(gameObject);
    }
}