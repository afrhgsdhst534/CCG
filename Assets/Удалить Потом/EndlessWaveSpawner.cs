using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemySpawner : MonoBehaviour
{
    public Row enemyRow;
    public GameObject[] lowTierSlimes;
    public GameObject[] midTierSlimes;
    public GameObject[] highTierSlimes;

    private BattleManager bm;

    [Header("Настройки сложности")]
    public int currentLevel = 0;
    public int maxCardsInRow = 9;

    [Range(0, 1)] public float baseMidChance = 0.1f;
    [Range(0, 1)] public float baseHighChance = 0.01f;

    void Start()
    {
        bm = FindFirstObjectByType<BattleManager>();
        bm.onEnd += Next; // Подпишемся сразу
        FillRow();
    }

    public void Next()
    {
        currentLevel++;
        FillRow();
        bm.onEnd += Next; // Обязательно подписка каждый раз!
    }

    void FillRow()
    {
        int freeSlots = maxCardsInRow - enemyRow.cards.Count;
        for (int i = 0; i < freeSlots; i++)
        {
            GameObject prefab = GetBalancedSlime();
            GameObject slime = Instantiate(prefab, enemyRow.transform);
            Card card = slime.GetComponent<Card>();
            card.parentRow = enemyRow;
            enemyRow.cards.Add(card);
        }
        enemyRow.Sync();
    }

    GameObject GetBalancedSlime()
    {
        float midChance = baseMidChance + currentLevel * 0.04f;
        float highChance = baseHighChance + currentLevel * 0.015f;

        float roll = Random.value;

        if (roll < highChance && highTierSlimes.Length > 0 && currentLevel >= 4)
        {
            return highTierSlimes[Random.Range(0, highTierSlimes.Length)];
        }
        else if (roll < highChance + midChance && midTierSlimes.Length > 0 && currentLevel >= 2)
        {
            return midTierSlimes[Random.Range(0, midTierSlimes.Length)];
        }
        else
        {
            return lowTierSlimes[Random.Range(0, lowTierSlimes.Length)];
        }
    }
}
