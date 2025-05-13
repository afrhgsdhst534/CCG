using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Card : MonoBehaviour
{
    public int attack = 2;
    public int health = 5;
    public int armor = 5;
    public Row parentRow;
    public BattleManager battleManager;
    public GameObject buffEffect;
    public bool isCool;
    public int PendingDamage { get; set; } = 0;
    private GameObject attackEffectPrefab; // Префаб частиц атаки
    public bool isSpell;
    public int mana;
    public virtual void Start()
    {
        parentRow = transform.parent.GetComponent<Row>();
        battleManager = FindFirstObjectByType<BattleManager>();
        attackEffectPrefab = battleManager.attackEffectPrefab;
    }
    public virtual IEnumerator OnPhase(BattlePhase phase)
    {
        switch (phase)
        {
            case BattlePhase.ApplyBuffs:
                yield return ApplyBuffs();
                break;
            case BattlePhase.UseAbilities:
                yield return UseAbilities();
                break;
            case BattlePhase.BeforeAttack:
                yield return PrepareAttackTarget();
                break;
            case BattlePhase.Attack:
                yield return DoAttack();
                break;
            case BattlePhase.ResolveDamage:
                yield return ResolveDamage();
                break;
            case BattlePhase.Cleanup:
                if (health <= 0)
                    Destroy(gameObject);
                break;
        }
    }
    public virtual IEnumerator ApplyBuffs() { yield break; }
    protected virtual IEnumerator PrepareAttackTarget() { yield break; }
    protected virtual IEnumerator DoAttack()
    {
        Card target = FindTarget();
        bool rotateEnemy = parentRow == battleManager.enemyRow;

        if (target != null)
        {
            yield return PrepareAttack(target, rotateEnemy);

            // Наносим урон немедленно
            target.TakeDamage(attack);

            // Обновление UI после атаки
            GetComponent<CardsInsides>()?.UpdateUI();
        }
        else
        {
            // Атака напрямую в игрока
            yield return PrepareAttackDummy(rotateEnemy);

            if (parentRow == battleManager.enemyRow)
            {
                battleManager.PlayerTakeDamage(attack);
            }
            else
            {
                battleManager.EnemyTakeDamage(attack);
            }

            GetComponent<CardsInsides>()?.UpdateUI();
        }

        yield return new WaitForSeconds(0.2f);
    }
    protected virtual IEnumerator ResolveDamage()
    {
        if (PendingDamage > 0)
        {
            health -= PendingDamage;
            PendingDamage = 0;
        }
        GetComponent<CardsInsides>()?.UpdateUI();
        yield break;
    }
    public Card FindTarget()
    {
        var enemyRow = parentRow == battleManager.enemyRow
            ? battleManager.playerRow
            : battleManager.enemyRow;
        int index = parentRow.cards.IndexOf(this);
        // Если у врага есть карта по индексу — атакуем её
        if (index >= 0 && index < enemyRow.cards.Count)
            return enemyRow.cards[index];
        // Если нет — атакуем крайнюю карту
        if (enemyRow.cards.Count > 0)
            return enemyRow.cards[enemyRow.cards.Count - 1];
        GetComponent<CardsInsides>()?.UpdateUI();
        // Если вообще никого нет — возвращаем null
        return null;
    }
    public IEnumerator PrepareAttack(Card target, bool rotate180 = false)
    {
        Vector3 startPos = transform.position;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 attackPos = transform.position + direction * 0.5f;
        attackPos.z = startPos.z; // фиксируем Z
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = rotate180 ? Quaternion.Euler(0, 0, 180) : startRot;
        if (rotate180)
        {
            float r = 0;
            while (r < 1)
            {
                r += Time.deltaTime * 4;
                transform.rotation = Quaternion.Lerp(startRot, targetRot, r);
                yield return null;
            }
        }
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            Vector3 move = Vector3.Lerp(startPos, attackPos, t);
            move.z = startPos.z;
            transform.position = move;
            yield return null;
        }
        PlayAttackEffect(target);
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            Vector3 move = Vector3.Lerp(attackPos, startPos, t);
            move.z = startPos.z;
            transform.position = move;
            yield return null;
        }
        if (rotate180)
        {
            float r = 0;
            while (r < 1)
            {
                r += Time.deltaTime * 4;
                transform.rotation = Quaternion.Lerp(targetRot, startRot, r);
                yield return null;
            }
        }
        GetComponent<CardsInsides>()?.UpdateUI();
    }
    public virtual IEnumerator UseAbilities()
    {
        yield break; // по умолчанию ничего не делает
    }
    public IEnumerator PrepareAttackDummy(bool isEnemy)
    {
        Vector3 startPos = transform.position;
        Vector3 attackDir = isEnemy ? Vector3.down : Vector3.up;
        Vector3 attackPos = startPos + attackDir * 0.5f;
        attackPos.z = startPos.z;
        Quaternion startRot = transform.rotation;
        Quaternion attackRot = isEnemy ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        if (isEnemy)
        {
            float r = 0;
            while (r < 1)
            {
                r += Time.deltaTime * 4;
                transform.rotation = Quaternion.Lerp(startRot, attackRot, r);
                yield return null;
            }
        }
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            Vector3 move = Vector3.Lerp(startPos, attackPos, t);
            move.z = startPos.z;
            transform.position = move;
            yield return null;
        }
        // 💥 Спавним эффект удара между атакующим и героем
        SpawnHeroHitEffect(startPos, isEnemy);
        // Возврат
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            Vector3 move = Vector3.Lerp(attackPos, startPos, t);
            move.z = startPos.z;
            transform.position = move;
            yield return null;
        }
        if (isEnemy)
        {
            float r = 0;
            while (r < 1)
            {
                r += Time.deltaTime * 4;
                transform.rotation = Quaternion.Lerp(attackRot, startRot, r);
                yield return null;
            }
        }
        GetComponent<CardsInsides>()?.UpdateUI();
    }
    public System.Action<Card> onDeath;
    public GameObject shieldEffect;
    public System.Action onPlayed;
    public virtual void OnCardPlayed() {
        onPlayed?.Invoke();
    }
    public virtual void TakeDamage(int amount)
    {
        if (isCool)
        {
            amount = Mathf.CeilToInt(amount / 2f); // Делим урон до вычитания
            GameObject fx = Instantiate(shieldEffect, transform.position, Quaternion.identity);
            fx.transform.position = new Vector3(transform.position.x,transform.position.y, 0); // фикс Z
            Destroy(fx, 0.5f);
        }
        health -= amount;

        Debug.Log($"{name} получает {amount} урона, осталось {health} HP");
        if (health <= 0)
        {
            Debug.Log($"{name} уничтожен");
            if (parentRow != null && parentRow.cards.Contains(this))
            {
                parentRow.cards.Remove(this);
            }
            onDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
    protected virtual void PlayAttackEffect(Card target)
    {
        if (attackEffectPrefab != null && target != null)
        {
            // Спавним эффект между атакующим и целью
            Vector3 spawnPos = Vector3.Lerp(transform.position, target.transform.position, 0.5f);
            GameObject effect = Instantiate(attackEffectPrefab, spawnPos, Quaternion.identity);
            Destroy(effect, 0.5f); // Автоматическое удаление после 2 секунд
        }
    }
    protected virtual void SpawnHeroHitEffect(Vector3 from, bool isEnemy)
    {
        // Позиция портрета героя (пример, можешь заменить на конкретный Transform)
        Vector3 heroPos = isEnemy
            ? parentRow.ebychka.position
            : parentRow.ebychka.position;
        Vector3 center = (from + heroPos) / 2f;
        center.z = 0;
        var fx = Instantiate(battleManager.attackEffectPrefab, center, Quaternion.identity);
        fx.transform.rotation = Quaternion.LookRotation(Vector3.forward, heroPos - from);
        Destroy(fx, 0.5f);
        // Можешь сюда добавить: fx.SetColor(...), fx.SetOwner(...), fx.Play();
    }
    public List<Card> GetNeighbors()
    {
        var neighbors = new List<Card>();
        if (parentRow != null)
        {
            int index = parentRow.cards.IndexOf(this);
            if (index > 0 && parentRow.cards[index - 1] != null)
                neighbors.Add(parentRow.cards[index - 1]);
            if (index < parentRow.cards.Count - 1 && parentRow.cards[index + 1] != null)
                neighbors.Add(parentRow.cards[index + 1]);
        }
        return neighbors;
    }
}