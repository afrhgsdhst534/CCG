using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelFive : MonoBehaviour
{
    public Card card;
    public Card card1;
    public Card card2;
    public Transform tr;
    BattleManager bm;
    private void Start()
    {
        Spawn();
        bm = FindFirstObjectByType<BattleManager>();
        bm.mana = 10;
        bm.onEnd += Next;
    }

    public void Spawn()
    {
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Next()
    {
        bm.mana = 10;

        if (bm.enemyHealth > 0) return;
        SceneManager.LoadScene("0 5");
    }
}