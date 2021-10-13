using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StatusIndicator : MonoBehaviour
{
    Image _image = null;
    Coroutine _currentFlashRoutine = null;
    Coroutine _currentShakeRoutine = null;
    private float CurrAlpha;
    // Start is called before the first frame update
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = new Color(1,0,0,0);
        CurrAlpha = _image.color.a;
    }

    public void ChangeTransparency(float newAlpha)
    {
        CurrAlpha = newAlpha;
        Color colorThisFrame = _image.color;
        colorThisFrame.a = CurrAlpha;
        _image.color = colorThisFrame;
    }

    public void ChangeColor(Color newColor)
    {
        CurrAlpha = _image.color.a;
        Color colorThisFrame = newColor;
        colorThisFrame.a = CurrAlpha;
        _image.color = colorThisFrame;
    }

    public void StartFlash(float secondForOneFlash, float maxAlpha, Color newColor, float endAlpha, Color endColor, int numOfFlash)
    {   
        //CurrAlpha = endAlpha
        _image.color = newColor;
        maxAlpha = Mathf.Clamp(maxAlpha, endAlpha, 1);
        if (_currentFlashRoutine != null)
            StopCoroutine(_currentFlashRoutine);
        _currentFlashRoutine = StartCoroutine(Flash(endColor, secondForOneFlash, maxAlpha, endAlpha, numOfFlash));
    }

    IEnumerator Flash(Color endColor, float SecondForOneFlash, float maxAlpha, float endAlpha, int numOfFlash)
    {
        // animate fade in
        float flashInDuration = SecondForOneFlash / 2;
        for (int i = numOfFlash; i > 0; i--)
        {
            for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
            {
                Color colorThisFrame = _image.color;
                colorThisFrame.a = Mathf.Lerp(endAlpha, maxAlpha, t / flashInDuration);
                _image.color = colorThisFrame;
                yield return null;
            }
            float flashOutDuration = SecondForOneFlash / 2;
            for (float t = 0; t <= flashOutDuration; t += Time.deltaTime)
            {
                Color colorThisFrame = _image.color;
                colorThisFrame.a = Mathf.Lerp(maxAlpha, endAlpha, t / flashInDuration);
                _image.color = colorThisFrame;
                yield return null;
            }
            if (i == 1)
            {
                CurrAlpha = _image.color.a;
                Color temp = endColor;
                temp.a = endAlpha;
                _image.color = temp;
            }
        }
        // animate fade out
    }

    public void StartShake(Camera camera, float ShakeDuration, float intensity)
    {
        if (_currentShakeRoutine != null)
            StopCoroutine(_currentShakeRoutine);
        _currentShakeRoutine = StartCoroutine(Shake(camera, ShakeDuration, intensity));
    }

    IEnumerator Shake(Camera camera, float ShakeDuration, float intensity)   
    {
        for (float t = 0; t <= ShakeDuration; t += Time.deltaTime)
        {
            camera.transform.position = new Vector3(camera.transform.position.x + Random.Range(-intensity, intensity),
                                                    camera.transform.position.y + Random.Range(-intensity, intensity),
                                                    camera.transform.position.z);
            yield return null;
        }
    }
}
