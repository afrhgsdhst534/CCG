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

            Debug.Log($"������ �������� {card.name}, ���� = {attack}, �������� = {health}");

            // ���������� �����������
            StartCoroutine(DestroyAfterFrame(card.gameObject));
        }

        GetComponent<CardsInsides>()?.UpdateUI();
    }

    private IEnumerator DestroyAfterFrame(GameObject obj)
    {
        yield return null; // ��������� 1 ����
        Destroy(obj);
    }
    public override IEnumerator ApplyBuffs()
    {
        StartCoroutine(AbsorbCards(GetNeighbors()));
        yield break; // ������ ������ �� ������ ��� �� ����
    }
}