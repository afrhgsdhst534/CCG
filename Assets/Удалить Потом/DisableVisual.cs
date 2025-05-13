using UnityEngine;
using UnityEngine.UI;

public class DisableVisual : MonoBehaviour
{
    public GameObject[] objs;int next;
    public Text text;
    public GameObject cnhtkf;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<BattleManager>().onEnd += End;

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(false);
        }
    }
    public void Ctrekrf()
    {
        next++;
        if (next == 1)
        {
            objs[0].SetActive(true);
            objs[1].SetActive(true);
            text.text = "У вас появилась карточка нажмите на нее. Не забывайте пока нажимать на стрелку :)";
        }
        if (next == 2)
        {
            objs[2].SetActive(true);
            objs[3].SetActive(true);
            text.text = "У вас появилась колода и мана, нажимая на крестик убирайте колоду чтобы смотреть на стол, если хотите выставить карту, то нажмите на карту в своей колоде";
        }
        if (next == 3)
        {
                objs[4].SetActive(true);
            objs[5].SetActive(true);
            text.text = "Слева сверху скорость боя, снизу сам бой. Нажмите на стрелочку в последний раз :(";
        }
        if (next == 4)
        {
            cnhtkf.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }

    public void End()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("0 1");
    }
}
