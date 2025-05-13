using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[ExecuteAlways]
public class Row : MonoBehaviour
{
    public int columns = 3;
    public bool allowCardSwap = true;

    public Vector2 cellSize = new Vector2(1f, 1f);
    public Vector2 spacing = new Vector2(0.1f, 0.1f);
    public GameObject obj;
    public bool battle; // false = Center, true = Left
    public bool freezeLayout = false;
    public List<Card> cards = new(); // сюда вручную добавляй карты при создании
    public TextMeshProUGUI statsUI;
    public Transform ebychka;
    public void FillRowWithLastCard()
    {
        if (lastPlayedCard == null) return;

        while (cards.Count < columns)
        {
            GameObject clone = Instantiate(lastPlayedCard.gameObject, transform);
            var cardComponent = clone.GetComponent<Card>();
            cardComponent.parentRow = this;
            cards.Add(cardComponent);
        }

        lastPlayedCard = null; // чтобы не повторялось
    }

    public void Sync()
    {
        List<Card> syncedList = new List<Card>();
        for (int i = 0; i <transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Card card = child.GetComponent<Card>();
            if (card != null)
            {
                syncedList.Add(card);
            }
        }

        cards = syncedList;
    }
    public Card lastPlayedCard;
    public Card SpawnCardFromPrefab(GameObject prefab, int index)
    {
        var instance = Instantiate(prefab, transform);
        var card = instance.GetComponent<Card>();
        cards.Insert(index, card);
        instance.transform.SetSiblingIndex(index);
        card.parentRow = this;
        return card;
    }
    public void AddCard(Card card)
    {
        cards.Add(card);
        card.transform.SetParent(transform);
    }
    public Card GetCardAt(int index) =>
        (index >= 0 && index < cards.Count) ? cards[index] : null;
    void Update()
    {
        Sync();
        if (!freezeLayout)
            UpdateLayout();
    }
    void UpdateLayout()
    {
        int count = transform.childCount;
        if (count == 0) return;
        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            int row = i / columns;
            int column = i % columns;
            int columnsInRow = Mathf.Min(columns, count - row * columns);
            float xOffset;
            if (battle)
            {
                transform.position = new Vector3(-6, transform.position.y, transform.position.z);
                xOffset = 0f + cellSize.x / 2f;
            }
            else
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
                float rowWidth = columnsInRow * cellSize.x + (columnsInRow - 1) * spacing.x;
                xOffset = -rowWidth / 2f + cellSize.x / 2f;
            }
            float x = xOffset + column * (cellSize.x + spacing.x);
            float y = -(row * (cellSize.y + spacing.y));
            child.localPosition = new Vector3(x, y, 0);
        }
    }
}