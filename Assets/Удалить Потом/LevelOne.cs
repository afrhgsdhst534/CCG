using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelOne : MonoBehaviour
{
    public Card card;
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
        Instantiate(card, tr);
        Instantiate(card, tr);
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Next()
    {
        if (bm.enemyHealth > 0) return;
        SceneManager.LoadScene("0 2");
    }
}