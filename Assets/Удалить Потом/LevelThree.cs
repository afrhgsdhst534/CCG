using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelThree : MonoBehaviour
{
    public Card card;
    public Card card1;
    public Card card2;
    public Card card3;
    public Transform tr;
    BattleManager bm;
    private void Start()
    {
        Spawn();
        bm = FindFirstObjectByType<BattleManager>();
        bm.mana = 5;
        bm.onEnd += Next;
    }
    public void Spawn()
    {
        Instantiate(card, tr);
        Instantiate(card1, tr);
        var a = Instantiate(card2, tr);
        a.health = 25;
        Instantiate(card3, tr);
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Next()
    {
        if (bm.enemyHealth > 0) return;
        SceneManager.LoadScene("0 4");
    }
}
