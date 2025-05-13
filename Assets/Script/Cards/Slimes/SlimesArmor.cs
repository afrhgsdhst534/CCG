using System.Collections;
using UnityEngine;
using TMPro;
public class SlimesArmor : Card
{
    public int wear = 3;
    public TextMeshProUGUI text;
    protected override IEnumerator DoAttack()
    {
        yield break;
    }
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public override IEnumerator UseAbilities()
    {
        wear--;
        text.text = wear.ToString();
        if (text.text == 0.ToString())
            text.enabled = true;
        foreach (var card in battleManager.playerRow.cards)
        {
            if (card != this && card != null)
            {
                card.isCool = true;
                // FX с фиксацией Z
                card.shieldEffect = buffEffect;
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (wear <= 0)
        {
            foreach (var card in battleManager.playerRow.cards)
            {
                if (card != this && card != null)
                {
                    card.isCool = false;
                }
            }
            parentRow.cards.Remove(this);
            Destroy(gameObject);
        }
    }
}