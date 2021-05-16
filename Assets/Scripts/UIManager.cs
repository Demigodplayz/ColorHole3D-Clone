/*
* Developed by Taygun SAVAŞ.
* www.taygunsavas.com
*
* Contact,
* info@taygunsavas.com
*/
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField, Tooltip("Canvas parent referansları")]
    private Transform inGameMenu;
    [SerializeField, Tooltip("Canvas parent referansları")]
    private Transform levelCompleted;
    [SerializeField, Tooltip("Canvas parent referansları")]
    private Transform levelFailed;
    [SerializeField, Header("Other References")]
    private Transform confettiObj;
    [SerializeField]
    private Transform progressBar1, progressBar2;
    private Slider barSlider;
    private int minValue = 0, maxValue, phase;
    [SerializeField, Tooltip("Level indicator texts")]
    private Text levelIndi1, levelIndi2;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        GameManager.OnLevelCompleted += OnLevelCompleted;
        GameManager.OnLevelStarted += OnLevelStarted;
        PlayerController.OnPlayerScored += CheckProgress;
        LevelActor.OnReqScoreChanged += SetMaxValue;
    }
    private void OnDisable()
    {
        GameManager.OnLevelCompleted -= OnLevelCompleted;
        GameManager.OnLevelStarted -= OnLevelStarted;
        PlayerController.OnPlayerScored -= CheckProgress;
        LevelActor.OnReqScoreChanged -= SetMaxValue;
    }
    private void OnLevelStarted(object[] args)
    {
        if ((bool)args[0])
        {
            inGameMenu.gameObject.SetActive(true);
            levelCompleted.gameObject.SetActive(false);
            levelFailed.gameObject.SetActive(false);
            confettiObj.gameObject.SetActive(false);
            ResetProgress();
            SetLevelTexts((int)args[1]);
        }
    }
    private void SetLevelTexts(int currentLevel)
    {
        levelIndi1.text = currentLevel.ToString();
        levelIndi2.text = (currentLevel + 1).ToString();
    }
    private void SetMaxValue(object[] args)
    {
        maxValue = (int)args[0];
        phase = (int)args[1];
    }
    private void OnLevelCompleted(object[] args)
    {
        if ((bool)args[0])
        {
            levelCompleted.gameObject.SetActive(true);
            confettiObj.gameObject.SetActive(true);
        }
        else
        {
            //LEVEL FAILED
            levelFailed.gameObject.SetActive(true);
        }
    }
    private void ResetProgress()
    {
        barSlider = progressBar1.GetComponent<Slider>();
        barSlider.maxValue = 0;
        barSlider.value = 0;
        barSlider.minValue = 0;
        barSlider = progressBar2.GetComponent<Slider>();
        barSlider.maxValue = 0;
        barSlider.value = 0;
        barSlider.minValue = 0;
        barSlider = null;
    }
    private void CheckProgress(int score)
    {
        if (phase == 0)
        {
            barSlider = progressBar1.GetComponent<Slider>();
            barSlider.maxValue = maxValue;
            barSlider.value = score;
            minValue = score;
        }
        else
        {
            barSlider = progressBar2.GetComponent<Slider>();
            barSlider.minValue = minValue;
            barSlider.maxValue = maxValue;
            barSlider.value = score;
        }
    }
}
