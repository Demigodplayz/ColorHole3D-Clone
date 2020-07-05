/*
* Developed by Taygun SAVAŞ.
* www.taygunsavas.com
*
* Contact,
* info@taygunsavas.com
*/
using System.Collections.Generic;
using UnityEngine;
public class LevelActor : MonoBehaviour
{
    //LevelEvents
    public delegate void LevelEvents(object[] args);
    public static event LevelEvents OnCheckPointReached;
    public static event LevelEvents OnReqScoreChanged;

    [System.Serializable]
    public struct CheckPoints
    {
        public Transform doorObj;
        public Transform nextPos;
    }
    [SerializeField, Tooltip("Checkpoint ayarları.Her kapı ve kapıdan geçtikten sonra gidilecek bir sonraki checkpoint pozisyonuna ait referans burada tanıtılmalı.")]
    private CheckPoints[] _levelInfo;


    private List<Transform> pointParents = new List<Transform>();
    private int requiredScore, pointIndex;

    private void OnEnable()
    {
        PlayerController.OnPlayerScored += CheckScore;
        GameManager.OnTransition += SetReqScore;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerScored -= CheckScore;
        GameManager.OnTransition -= SetReqScore;
    }
    private void Start()
    {
        SetPointParents();
        SetReqScore(new object[] { true, 0 });
    }
    private void SetPointParents()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Points") && !pointParents.Contains(transform.GetChild(i)))
                pointParents.Add(transform.GetChild(i));
        }
    }
    private void SetReqScore(object[] args)
    {
        if ((bool)args[0])
        {
            pointIndex = args.Length > 1 ? (int)args[1] : pointIndex + 1;
            requiredScore += pointParents[pointIndex].childCount;
            OnReqScoreChanged(new object[] { requiredScore, pointIndex });
        }
    }
    private void CheckScore(int score)
    {
        if (score == requiredScore)
        {
            if (_levelInfo.Length > pointIndex)
            {
                OnCheckPointReached(new object[] { true, _levelInfo[pointIndex].doorObj, _levelInfo[pointIndex].nextPos });
            }
            else
                OnCheckPointReached(new object[] { false });
        }
    }
}
