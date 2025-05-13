using UnityEngine;
using System.Collections;

public class SaintSlime : Card
{
    public int maxResurrects = 3;

    public override IEnumerator ApplyBuffs()
    {
        var neighbors = GetNeighbors();
        maxResurrects--;
        foreach (var neighbor in neighbors)
        {
            if (neighbor == null) continue;

            // ������������� �� ������ ������, �������� �� null
            if (neighbor.onDeath != null)
            {
                neighbor.onDeath -= OnNeighborDeath; // ������� ������ ��������, ���� ��� ����
            }
            neighbor.onDeath += OnNeighborDeath;
        }

        yield break;
    }

    private void OnNeighborDeath(Card deadNeighbor)
    {
        try
        {
            // ���������, ���������� �� deadNeighbor, ���� ������ ��������� � �������
            if (deadNeighbor == null)
            {
                return;
            }

            // ��������, ��� �� ��������� ������
            if (deadNeighbor.gameObject == null)
            {
                return;
            }

            if (maxResurrects <= 0) return;

            int maxStat = Mathf.Max(deadNeighbor.attack, deadNeighbor.health);

            // ��������� ��������� ������
            GameObject clone = Instantiate(deadNeighbor.gameObject);
            clone.transform.SetParent(parentRow.transform, false);

            // ��������� � ������ ������� � ������ � ��������
            int index = deadNeighbor.transform.GetSiblingIndex();
            clone.transform.SetSiblingIndex(index);

            Card resurrected = clone.GetComponent<Card>();
            resurrected.attack = maxStat;
            resurrected.health = maxStat;
            resurrected.parentRow = parentRow;

            parentRow.cards.Insert(index, resurrected);

            // ������ ����������� (�����������)
            GameObject fx = Instantiate(buffEffect, resurrected.transform.position, Quaternion.identity);
            fx.transform.position = new Vector3(resurrected.transform.position.x, resurrected.transform.position.y, 0);

            Destroy(fx, 0.9f);

        }
        catch 
        {

        }

    }
}
