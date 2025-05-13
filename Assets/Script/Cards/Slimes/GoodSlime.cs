using System.Collections;
using UnityEngine;
public class GoodSlime : Card
{
    public override void Start()
    {
        base.Start();
    }
    public override IEnumerator ApplyBuffs()
    {
        foreach (var neighbor in GetNeighbors())
        {
            if (neighbor == null) continue;
            GameObject fx = Instantiate(buffEffect, neighbor.transform.position, Quaternion.identity);
            fx.transform.position = new(fx.transform.position.x, fx.transform.position.y, 0);

            Destroy(fx, 0.9f); // уничтожить после проигрывания

            neighbor.health += health;
            Debug.Log($"{name} бафнул {neighbor.name} на {health}");
            if (this != null && gameObject != null)
            {
                var ui = GetComponent<CardsInsides>();
                if (ui != null) ui.UpdateUI();
            }
            yield return new WaitForSeconds(0.5f);
        }
        if (this != null && gameObject != null)
        {
            var ui = GetComponent<CardsInsides>();
            if (ui != null) ui.UpdateUI();
        }
        yield return new WaitForSeconds(0.3f);
    }
}