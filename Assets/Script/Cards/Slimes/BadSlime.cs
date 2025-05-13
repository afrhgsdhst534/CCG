using UnityEngine;
using System.Collections;
public class BadSlime : Card
{
    public override void Start()
    {
        base.Start();
    }

    public override IEnumerator ApplyBuffs()
    {
        var neighbors = GetNeighbors();
        foreach (var neighbor in neighbors)
        {
            if (this == null || neighbor == null) yield break;
            GameObject fx = Instantiate(buffEffect, neighbor.transform.position, Quaternion.identity);
            fx.transform.position = new(fx.transform.position.x, fx.transform.position.y, 0);

            Destroy(fx, 0.9f); // уничтожить после проигрывания

            // Пример безопасного доступа
            // Важно: не трогай "this.name", если this == null
            neighbor.attack += attack;
                Debug.Log($"{gameObject.name} бафнул {neighbor.gameObject.name} на {attack}");
                GetComponent<CardsInsides>()?.UpdateUI();
                yield return new WaitForSeconds(0.5f);
         
        }
        yield return new WaitForSeconds(0.3f);
    }
}