using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class StatusIndicator : MonoBehaviour
{
    Image _image = null;
    Coroutine _currentFlashRoutine = null;
    private float CurrAlpha = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeTransparency(float newAlpha)
    {
        CurrAlpha = newAlpha;
        Color colorThisFrame = _image.color;
        colorThisFrame.a = CurrAlpha;
        _image.color = colorThisFrame;
    }

    public void StartFlash(float secondForOneFlash, float maxAlpha, Color newColor, int numOfFlash)
    {
        _image.color = newColor;

        maxAlpha = Mathf.Clamp(maxAlpha, CurrAlpha, 1);
        if (_currentFlashRoutine != null)
            StopCoroutine(_currentFlashRoutine);
        _currentFlashRoutine = StartCoroutine(Flash(secondForOneFlash, maxAlpha, numOfFlash));
        
    }
    
    IEnumerator Flash(float SecondForOneFlash, float maxAlpha, int numOfFlash)
    {
        // animate fade in
        float flashInDuration = SecondForOneFlash / 2;
        for (int i = numOfFlash; i > 0; i--)
        {
            for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
            {
                Color colorThisFrame = _image.color;
                colorThisFrame.a = Mathf.Lerp(CurrAlpha, maxAlpha, t / flashInDuration);
                _image.color = colorThisFrame;
                yield return null;
            }
            float flashOutDuration = SecondForOneFlash / 2;
            for (float t = 0; t <= flashOutDuration; t += Time.deltaTime)
            {
                Color colorThisFrame = _image.color;
                colorThisFrame.a = Mathf.Lerp(maxAlpha, CurrAlpha, t / flashInDuration);
                _image.color = colorThisFrame;
                yield return null;
            }
        }

        // animate fade out
    }

}
