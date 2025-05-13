using System.Collections.Generic;
using UnityEngine;
public class Test : MonoBehaviour
{
    [System.Serializable]
    public class Slime
    {
        public Slime(string name, float baseWeight)
        {
            this.name = name;
            this.baseWeight = baseWeight;
            this.weight = baseWeight;
        }
        public string name;
        public float weight;
        public float baseWeight;
    }
    public List<Slime> slimes = new List<Slime>();
    [Header("Настройки изменения весов")]
    public float boostFactor = 1.2f;     
    public float reduceFactor = 0.8f;    
    public int powerThreshold = 3;        
    void Start()
    {
        slimes = new List<Slime>
        {
            new Slime("BlueSlime", 50f),
            new Slime("BlackSlime", 22f),
            new Slime("RedSlime", 14f),
            new Slime("PurpleSlime", 1.455f),
            new Slime("LonlySlime", 1.005f),
            new Slime("ImposibleSlime", 0.802f),
            new Slime("MonsterSlime", 0.406f),
            new Slime("SlimeLord", 0.182f),
        };
        ResetWeights();
        DrawSlime();
        ShowChances();
    }
    public void ResetWeights()
    {
        foreach (var slime in slimes)
            slime.weight = slime.baseWeight;
    }
    public void ShowChances()
    {
        float totalWeight = GetTotalWeight();
        for (int i = 0; i < slimes.Count; i++)
        {
            float chance = (slimes[i].weight / totalWeight) * 100f;
        }
    }
    [ContextMenu("Draw Slime")]
    public void DrawSlime()
    {
        if (slimes[3].weight>=29)
        {
            powerThreshold=4;
        }
        if (slimes[4].weight >= 40)
        {
            powerThreshold = 5;
        }
        float totalWeight = GetTotalWeight();
        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        for (int i = 0; i < slimes.Count; i++)
        {
            cumulative += slimes[i].weight;
            if (roll <= cumulative)
            {
                Debug.Log($"🎉 Выпал: {slimes[i].name}");
                // после дропа усиливаем сильных и ослабляем слабых
                for (int j = 0; j < slimes.Count; j++)
                {
                    if (j >= powerThreshold)
                        slimes[j].weight *= boostFactor;
                    else
                        slimes[j].weight *= reduceFactor;
                }
                NormalizeWeights(); // всегда нормализуем
                ShowChances();
                return;
            }
        }
    }
    float GetTotalWeight()
    {
        float total = 0f;
        foreach (var slime in slimes)
            total += slime.weight;
        return total;
    }
    void NormalizeWeights()
    {
        float total = GetTotalWeight();
        foreach (var slime in slimes)
            slime.weight = (slime.weight / total) * 100f;
    }
}