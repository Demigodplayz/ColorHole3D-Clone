/*
* Developed by Taygun SAVAŞ.
* www.taygunsavas.com
*
* Contact,
* info@taygunsavas.com
*/
using DG.Tweening;
using System.Collections;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    //GameManager Events
    public delegate void GameEvents(object[] args);
    public static event GameEvents OnTransition;
    public static event GameEvents OnLevelCompleted;
    public static event GameEvents OnLevelStarted;


    public static GameManager instance;
    private Vector3 cameraStartPos = new Vector3(0f, 9.4f, -8.14f);
    [SerializeField]
    private Transform[] levels;
    private Transform player, activeLevel;
    private int currentLevel = 0;
    [SerializeField]
    private Color[] backgroundColor;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        LevelActor.OnCheckPointReached += NextCheckpoint;
    }
    private void OnDisable()
    {
        LevelActor.OnCheckPointReached -= NextCheckpoint;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        currentLevel = PlayerPrefs.GetInt("Level");
        InitLevel(currentLevel);
    }

    public void GetPlayerInfo(Transform playerInfo)
    {
        player = playerInfo;
    }
    private void InitLevel(int index)
    {
        Camera.main.backgroundColor = backgroundColor[Random.Range(0, backgroundColor.Length)];
        OnLevelStarted(new object[] { true, currentLevel + 1 });
        Camera.main.transform.position = cameraStartPos;
        if (activeLevel)
        {
            Destroy(activeLevel.gameObject);
            activeLevel = null;
        }
        index = index < levels.Length ? index : Random.Range(0, levels.Length);
        activeLevel = Instantiate(levels[index]);
    }
    private void NextCheckpoint(object[] args)
    {
        if (!(bool)args[0])
        {
            //Level completed.
            OnLevelCompleted(new object[] { true });
            currentLevel++;
            PlayerPrefs.SetInt("Level", currentLevel);
        }
        else
        {
            //Move to next checkpoint
            Transform doorObj = (Transform)args[1];
            Transform nextPos = (Transform)args[2];
            OnTransition(new object[] { true });
            doorObj.DOMoveY(-2f, .7f).OnComplete(() => doorObj.gameObject.SetActive(false));
            player.DOLocalMoveX(0f, .5f).OnComplete(() =>
            {
                Camera.main.transform.DOMoveZ(nextPos.localPosition.z - 1.9f, 5f);
                player.DOLocalMoveZ(nextPos.localPosition.z, 5f).OnKill(() => OnTransition(new object[] { false }));
            });

        }
    }

    public IEnumerator LevelFailed()
    {
        //Level failed delay
        yield return new WaitForSeconds(.2f);
        CameraShake.instance.ShakeIt();
        OnLevelCompleted(new object[] { false });
    }

    //UI Button Events
    public void ContinueButtonHandler()
    {
        InitLevel(currentLevel);
    }
    public void TryAgainBtnHandler()
    {
        InitLevel(currentLevel);
    }
}
