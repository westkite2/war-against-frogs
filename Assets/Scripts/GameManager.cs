using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject HPbar;
    public GameObject GameOverUI;
    public GameObject PauseUI;
    public GameObject Damage;
    private Image HPfill;
    public int hp;
    public Text ScoreUI;
    public int score;
    public bool GAMEOVER = false;

    // Start is called before the first frame update
    void Start()
    {
        GameOverUI.SetActive(false);
        PauseUI.SetActive(false);
        HPfill = HPbar.transform.GetChild(1).GetComponent<Image>();
        Damage = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        hp = 100;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HPfill.fillAmount = hp / 100f;
        ScoreUI.text = score.ToString();
        if (hp <= 0)
        {
            Debug.Log("Game Over!");
            GAMEOVER = true;
            GameOverUI.SetActive(true);
        }

    }
}
