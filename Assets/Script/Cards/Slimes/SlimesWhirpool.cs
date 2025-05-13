using UnityEngine;
using System.Collections;
public class SlimeWhirlpool : Card
{
    public GameObject stealEffect;
    protected override IEnumerator DoAttack()
    {
        yield break;
    }
    public override IEnumerator UseAbilities()
    {
        Row enemyRow = battleManager.enemyRow;
        Row playerRow = battleManager.playerRow;
        if (enemyRow.cards.Count == 0 || playerRow.cards.Count >= playerRow.columns)
            yield break;
        Card target = enemyRow.cards[Random.Range(0, enemyRow.cards.Count)];
        if (stealEffect != null)
        {
            GameObject fx = Instantiate(stealEffect, target.transform.position, Quaternion.identity);
            fx.transform.position = new Vector3(fx.transform.position.x, fx.transform.position.y-1, 0);
            Destroy(fx, 0.6f);
        }
        yield return new WaitForSeconds(0.5f);
        enemyRow.cards.Remove(target);
        playerRow.cards.Add(target);
        target.parentRow = playerRow;
        Vector3 newPos = playerRow.transform.position + new Vector3(1.5f * (playerRow.cards.Count - 1), 0, 0);
        target.transform.position = newPos;
        target.transform.SetParent(playerRow.transform);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}