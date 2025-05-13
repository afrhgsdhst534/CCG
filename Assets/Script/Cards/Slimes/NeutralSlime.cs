using UnityEngine;
using System.Collections;
public class NeutralSlime : Card
{
    public override IEnumerator ApplyBuffs()
    {
        foreach (var neighbor in GetNeighbors())
        {
            GameObject fx = Instantiate(buffEffect, neighbor.transform.position, Quaternion.identity);
            fx.transform.position = new(fx.transform.position.x,fx.transform.position.y,0);
            Destroy(fx, 0.9f); // уничтожить после проигрывания
            neighbor.health += 5; 
            neighbor.attack += 5; 
            Debug.Log($"{name} бафнул {neighbor.name} на 5");
            GetComponent<CardsInsides>()?.UpdateUI();
            yield return new WaitForSeconds(0.5f);
        }
        GetComponent<CardsInsides>()?.UpdateUI();
        yield return new WaitForSeconds(0.3f); 
    }
}