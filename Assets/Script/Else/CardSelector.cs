using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    public Card card;
    private bool isBlack;
    private Button button;
    public bool inDeck;
    Deck deck;
    GameObject spawnedInDeck;

    private void Start()
    {
        button = GetComponent<Button>();
        deck = FindFirstObjectByType<Deck>();

        Image[] images = card.GetComponentsInChildren<Image>(true);
        Image targetImage = images.FirstOrDefault(img => img.transform != card.transform); // игнорируем корневой Image

        if (card.isSpell && targetImage != null)
        {
            GetComponent<Image>().sprite = targetImage.sprite;
        }
        else
        {
            var insides = card.GetComponent<CardsInsides>();
            if (insides != null && insides.image != null)
                GetComponent<Image>().sprite = insides.image.sprite;
        }
        if (!inDeck)
            button.onClick.AddListener(OnClick);
        else
            button.onClick.AddListener(Create);
    }

    public void OnClick()
    {
        if (!isBlack && deck.deckCards.Count >= 9) return;

        isBlack = !isBlack;
        GetComponent<Image>().color = isBlack ? new Color(0.6f, 0.6f, 0.6f) : Color.white;

        if (isBlack)
        {
            deck.deckCards.Add(card);
            spawnedInDeck = Instantiate(this.gameObject, deck.transform);
            spawnedInDeck.GetComponent<Tooltip>().offset = new(-300,200);
            spawnedInDeck.GetComponent<Image>().color =  Color.white;
            spawnedInDeck.GetComponent<CardSelector>().inDeck = true;
        }
        else
        {
            deck.deckCards.Remove(card);
            if (spawnedInDeck != null)
                Destroy(spawnedInDeck);
        }
    }
    public void Create()
    {
        Transform row = card.isSpell ? deck.spellsRow : deck.playerRow;
        BattleManager bm = FindFirstObjectByType<BattleManager>();

        if (row.GetComponent<Row>().cards.Count >= 9 || card.mana > bm.mana)
            return;

        bm.mana -= card.mana;
        var instance = Instantiate(card.gameObject, row);
        var cardComponent = instance.GetComponent<Card>();
        row.GetComponent<Row>().lastPlayedCard = cardComponent;
    }
}
