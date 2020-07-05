/*
* Developed by Taygun SAVAŞ.
* www.taygunsavas.com
*
* Contact,
* info@taygunsavas.com
*/
using DG.Tweening;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    //Player Events
    public delegate void PlayerPointer(int score);
    public static event PlayerPointer OnPlayerScored;

    private Vector3 _startPos;
    private bool dragging;
    [SerializeField]
    private int _layerOnEnter, _layerOnExit;
    private bool inTransition = false;
    private Transform platform;
    private float minZ, maxZ, minX, maxX;
    private int _score;

    private void OnEnable()
    {
        GameManager.instance.GetPlayerInfo(transform);
        GameManager.OnTransition += SetTransitionStatus;
        GameManager.OnLevelCompleted += SetTransitionStatus;
        Pointer.OnChildDestoyed += SetScore;
    }
    private void OnDisable()
    {
        GameManager.OnTransition -= SetTransitionStatus;
        GameManager.OnLevelCompleted -= SetTransitionStatus;
        Pointer.OnChildDestoyed -= SetScore;
    }

    private void SetScore()
    {
        _score++;
        OnPlayerScored(_score);
    }

    private void GetLimits()
    {
        //Set movement limits
        maxZ = platform.GetComponent<BoxCollider>().bounds.max.z - 1f;
        minZ = platform.GetComponent<BoxCollider>().bounds.min.z + 1f;
        maxX = platform.GetComponent<BoxCollider>().bounds.max.x - 1f;
        minX = platform.GetComponent<BoxCollider>().bounds.min.x + 1f;
    }
    private void SetTransitionStatus(object[] args)
    {
        inTransition = (bool)args[0];
        dragging = false;
    }
    private void OnMouseDown()
    {
        if (inTransition)
            return;
        _startPos = transform.position;
        dragging = true;
    }
    private void OnMouseUp()
    {
        if (inTransition)
            return;
        dragging = false;
    }
    private void Update()
    {
        if (!dragging)
            return;
        Vector3 distanceToScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen.z));
        pos_move.x = pos_move.x < maxX ? pos_move.x : maxX;
        pos_move.x = pos_move.x > minX ? pos_move.x : minX;
        pos_move.z = pos_move.z < maxZ ? pos_move.z : maxZ;
        pos_move.z = pos_move.z > minZ ? pos_move.z : minZ;
        transform.position = new Vector3(pos_move.x, _startPos.y, pos_move.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        //InHole
        if (other.CompareTag("Points") || other.CompareTag("Balls"))
        {
            other.gameObject.layer = _layerOnEnter;
            Destroy(other.gameObject, .35f);
        }
        else if (other.CompareTag("Platform"))
        {
            platform = other.transform;
            GetLimits();
        }
        else if (other.CompareTag("Block"))
        {
            if (inTransition)
                return;
            //Level Failed
            other.gameObject.layer = _layerOnEnter;
            Destroy(other.gameObject, .8f);
            GameManager.instance.StartCoroutine("LevelFailed");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        //OnTable
        if (other.CompareTag("Points") || other.CompareTag("Balls"))
            other.gameObject.layer = _layerOnExit;
    }
}
