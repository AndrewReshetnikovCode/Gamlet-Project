using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMark : MonoBehaviour
{
    [SerializeField] List<Image> _hitMarks;

    public float displayTime;
    public float fadeTime;
    public float hitTime;
    public int currentMarks;
    public int MaxMarks => _hitMarks.Count;
    float _currentHitTimer;
    Color _baseColor;
    float transparentChannel = 1;

    Coroutine _coroutine;

    private void Start()
    {
        if (_hitMarks.Count >= 1)
            _baseColor = _hitMarks[0].color;
    }
    public void Display()
    {
        foreach (var item in _hitMarks)
        {
            item.color = _baseColor;
            item.gameObject.SetActive(false);
        }

        //currentMarks++;
        int marksToDisplay = currentMarks >= MaxMarks ? MaxMarks : currentMarks;
        for (int i = 0; i < marksToDisplay; i++)
        {
            _hitMarks[i].gameObject.SetActive(true);
        }

        StopAllCoroutines();
        StartCoroutine(nameof(FadeMarks));
    }
    IEnumerator FadeMarks()
    {
        yield return new WaitForSeconds(displayTime);
        float transparentChannel = 1;
        while (transparentChannel >= 0)
        {
            transparentChannel -= 1 / fadeTime * Time.deltaTime;
            foreach (var item in _hitMarks) item.color = new Color(_baseColor.r, _baseColor.g, _baseColor.b, transparentChannel);
            yield return null;
        }
        foreach (var item in _hitMarks) item.color = new Color(_baseColor.r, _baseColor.g, _baseColor.b, 0);
        ResetHitTimer();
    }
    public void ResetHitTimer()
    {
        _currentHitTimer = 0;
        currentMarks = 0;
    }
}
