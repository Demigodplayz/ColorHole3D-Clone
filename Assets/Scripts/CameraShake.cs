/*
* Developed by Taygun SAVAŞ.
* www.taygunsavas.com
*
* Contact,
* info@taygunsavas.com
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 originalPos;
    private bool isShaking;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (!isShaking)
        {
            return;
        }

        if (shakeDuration > 0)
        {
            Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            Camera.main.transform.localPosition = originalPos;
            isShaking = false;
        }
    }

    public void ShakeIt(float duration = 0.1f)
    {
        originalPos = Camera.main.transform.localPosition;
        isShaking = true;
        shakeDuration = duration;
    }
}
