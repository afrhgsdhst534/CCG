using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CardsInsides : MonoBehaviour
{
    public TextMeshProUGUI attack; 
    public TextMeshProUGUI hp; 
    public TextMeshProUGUI armor; 
    public TextMeshProUGUI stats;
    public Image image;
    public Card card;
    public VolumeControler volumeControler;
    void Start()
    {
        stats.transform.parent.gameObject.SetActive(false);
        armor.transform.parent.gameObject.SetActive(false);
        card = GetComponent<Card>();
        volumeControler = FindFirstObjectByType<VolumeControler>();
    }
    private void Update()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        if (card == null) return;
        attack.text = card.attack.ToString();
        hp.text = card.health.ToString();
        stats.transform.parent.gameObject.SetActive(false); // отключено по умолчанию
    }
}