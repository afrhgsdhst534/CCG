using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Deck : MonoBehaviour
{
    public Transform playerRow;
    public Transform spellsRow;
    public GameObject cardPrefab;
    public const int maxDeckSize = 9;
    public List<Card> deckCards = new List<Card>(); 
    private void Start()
    {
        BattleManager manager = FindFirstObjectByType<BattleManager>();
        playerRow = manager.playerRow.transform;
        spellsRow = manager.spellsRow.transform;
    }
}