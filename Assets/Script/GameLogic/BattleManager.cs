using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
public enum BattlePhase
{
    StartTurn,
    ApplyBuffs,
    UseAbilities,
    BeforeAttack,
    Attack,
    AfterAttack,
    ResolveDamage,
    Cleanup,
    EndTurn
}
public class BattleManager : MonoBehaviour
{
    public GameObject attackEffectPrefab; // Префаб частиц атаки
    public TextMeshProUGUI manaUI;
    public int playerHealth = 20;
    public int enemyHealth = 20;
    public int mana;
    public GameObject playButton;
    public Row playerRow;
    public Row spellsRow;
    public Row enemyRow;
    public TextMeshProUGUI speedUI;
    public float timeScale;
    [Header("ToolTips")]
    public GameObject tooltipImage;
    public Text tooltipText;
    private void Start()
    {
        timeScale = Time.timeScale;
    }
    public void Speed()
    {
        timeScale *= 2;
        if (timeScale > 8)
        {
            timeScale = 1;
        }
    }
    private void Update()
    {
        Time.timeScale = timeScale;
        speedUI.text = $"X{timeScale:0.##}";
        enemyRow.statsUI.text = enemyHealth.ToString();
        playerRow.statsUI.text = playerHealth.ToString();
        manaUI.text = mana.ToString();
    }
    public Action onEnd;
    private IEnumerator RunBattle()
    {
        foreach (BattlePhase phase in System.Enum.GetValues(typeof(BattlePhase)))
        {
            yield return RunPhase(phase);
        }
        onEnd?.Invoke();
        playButton.SetActive(true);
        playerRow.battle = false;
        enemyRow.battle = false;
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private IEnumerator RunPhase(BattlePhase phase)
    {
        Debug.Log("Phase: " + phase);
        // Копируем список, чтобы избежать ошибки при его изменении
        var playerCardsCopy = playerRow.cards.ToList();
        for (int i = 0; i < playerCardsCopy.Count; i++)
        {
            var card = playerCardsCopy[i];
            if (card != null)
                yield return card.OnPhase(phase);
        }
        var epellsCardsCopy = spellsRow.cards.ToList();
        for (int i = 0; i < epellsCardsCopy.Count; i++)
        {
            var card = epellsCardsCopy[i];
            if (card != null)
                yield return card.OnPhase(phase);
        }
        var enemyCardsCopy = enemyRow.cards.ToList();
        for (int i = 0; i < enemyCardsCopy.Count; i++)
        {
            var card = enemyCardsCopy[i];
            if (card != null)
                yield return card.OnPhase(phase);
        }
        RemoveMissingScripts(playerRow.cards);
        RemoveMissingScripts(enemyRow.cards);
        yield return new WaitForSeconds(0.5f);
    }
    void RemoveMissingScripts<T>(List<T> list) where T : MonoBehaviour
    {
        list.RemoveAll(item => item == null || !IsScriptAttached(item));
    }
    bool IsScriptAttached(MonoBehaviour script)
    {
        // Если у объекта нет компонента (скрипта) MonoBehaviour, то вернется false 
        return script != null && !string.IsNullOrEmpty(script.GetType().ToString());
    }
    public void StartBattle()
    {
        playerRow.battle = true;
        enemyRow.battle = true;
        StartCoroutine(RunBattle());
    }
    public void PlayerTakeDamage(int amount)
    {
        playerHealth -= amount;

        Debug.Log("[BattleManager] Игрок получил урон: " + amount);
    }
    public void EnemyTakeDamage(int amount)
    {
        enemyHealth -= amount;
        Debug.Log("[BattleManager] Враг получил урон: " + amount);
    }
}