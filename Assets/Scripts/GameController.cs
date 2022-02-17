using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;

    [Header("Points")]
    [SerializeField] private Text PointText;
    private int pointValue = 0;

    [Header("Heals")]
    [SerializeField] private Image[] HealVisual = new Image[3];
    [SerializeField] private Sprite offHealSprite;
    [SerializeField] private int healCount = 3;

    [Header("UI Menu Elements")]
    [SerializeField] private GameObject startLevelPanel;
    [SerializeField] private GameObject[] GamePlayedPanels = new GameObject[1];
    [SerializeField] private GameObject winGamePanel;
    [SerializeField] private GameObject loseGamePanel;

    [Header("Audio")]
    [SerializeField] private AudioClip backgroundClip;
    [SerializeField] private AudioClip pointUpdateAudio;
    [SerializeField] private AudioClip damageAudio;
    [SerializeField] private AudioClip trackAudio;

    [Header("ID следующего уровня")]
    [SerializeField] private int nextLevelID = 0;

    [SerializeField] private UnityEvent gameStarted;
    [SerializeField] private UnityEvent gameEnded;

    [HideInInspector] public bool gameIsPlayed = false;

    private void Awake()
    {
        instance = this;

        healCount = HealVisual.Length;

        if (PointText != null) PointText.text = "0";
        pointValue = 0;

        //отключаем лишние панели и включаем только стартовую
        if (startLevelPanel != null) startLevelPanel.SetActive(true);
        if (GamePlayedPanels.Length > 0)
            for (int i = 0; i < GamePlayedPanels.Length; i++)
                GamePlayedPanels[i].SetActive(false);

        if (winGamePanel != null) winGamePanel.SetActive(false);
        if (loseGamePanel != null) loseGamePanel.SetActive(false);
    }

    private void Start()
    {
        if (SoundManagerAllControll.Instance && backgroundClip)
        {
            SoundManagerAllControll.Instance.BackgroundClipPlay(backgroundClip);
            SoundManagerAllControll.Instance.ClipLoopAndPlay(trackAudio);
        }
    }

    [SerializeField] private Color deactiveHealColor = new Color();
    public void SetDamage()
    {
        healCount -= 1;
        HealVisual[healCount].color = deactiveHealColor;

        if (SoundManagerAllControll.Instance && damageAudio != null) SoundManagerAllControll.Instance.ClipPlay(damageAudio);

        if (healCount <= 0)
            GameEnd(false);
    }

    public void UpdatePoint(int point)
    {
        if (!gameIsPlayed) return;

        pointValue += point;
        if (PointText != null) PointText.text = pointValue.ToString();

        if (SoundManagerAllControll.Instance && pointUpdateAudio != null) SoundManagerAllControll.Instance.ClipPlay(pointUpdateAudio);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelID);
    }

    public void GameStarted()
    {
        StartCoroutine(StartGameCoroutine());
       /* gameIsPlayed = true;

        if (startLevelPanel != null) startLevelPanel.SetActive(false);
        if (GamePlayedPanels.Length > 0)
            for (int i = 0; i < GamePlayedPanels.Length; i++)
                GamePlayedPanels[i].SetActive(true);

        if (PointText != null) PointText.text = "0";
        pointValue = 0;

        gameStarted.Invoke();*/
    }

    [SerializeField] private Animator uiAnim;
    private IEnumerator StartGameCoroutine()
    {
        if (uiAnim != null) uiAnim.SetTrigger("Start");

        yield return new WaitForSeconds(0.5f);

        gameIsPlayed = true;

        if (startLevelPanel != null) startLevelPanel.SetActive(false);
        if (GamePlayedPanels.Length > 0)
            for (int i = 0; i < GamePlayedPanels.Length; i++)
                GamePlayedPanels[i].SetActive(true);

        if (PointText != null) PointText.text = "0";
        pointValue = 0;

        gameStarted.Invoke();
    }

    public void GameEnd(bool win = true)
    {
        gameIsPlayed = false;
        gameEnded.Invoke();

        for (int i = 0; i < GamePlayedPanels.Length; i++)
            GamePlayedPanels[i].SetActive(false);

        if (win)
        {
            if (winGamePanel != null)
                winGamePanel.SetActive(true);
        }
        else
        {
            if (loseGamePanel != null)
                loseGamePanel.SetActive(true);
        }
    }
}