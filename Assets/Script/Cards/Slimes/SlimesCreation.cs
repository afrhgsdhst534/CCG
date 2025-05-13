using UnityEngine;
using System.Collections;
public class SlimesCreation : Card
{
    protected override IEnumerator DoAttack()
    {
        yield break;
    }
    public override IEnumerator UseAbilities()
    {
        if (battleManager.playerRow.lastPlayedCard == null) yield break;
        battleManager.playerRow.FillRowWithLastCard();
        Destroy(gameObject);
        yield break;
    }
}