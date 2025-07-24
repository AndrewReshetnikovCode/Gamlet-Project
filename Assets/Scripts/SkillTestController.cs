using UnityEngine;
using TMPro;

public class SkillTestController : MonoBehaviour
{
    public TMP_Text childCountText;
    public TMP_Text timerText;

    public float startDelay;
    public float initialTime = 10f;
    [SerializeField] DummyRespawn _dr;
    [SerializeField] KeyCode _resetKey;
    float _currentTime;
    float _currentDelay;
    bool _isCounting = false;

    private void Start()
    {
        ResetTimer();
        UpdateChildCount();
        UpdateTimerText();
    }

    private void Update()
    {
        _currentDelay -= Time.deltaTime;
        if (_currentDelay > 0)
        {
            return;
        }
        if (Input.GetKeyDown(_resetKey))
        {
            ResetTimer();
            StartTimer();
            _dr.ForceRespawn();
        }
        UpdateChildCount();

        if (_isCounting)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0f)
            {
                _currentTime = 0f;
                _isCounting = false;
            }

            UpdateTimerText();
        }
    }

    private void UpdateChildCount()
    {
        if (_dr.CurrentContainer != null && childCountText != null)
        {
            int count = _dr.CurrentContainer.childCount;
            childCountText.text = count.ToString();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            string currentStr = _currentTime.ToString("F2");
            string initialStr = initialTime.ToString("F0");
            timerText.text = string.Format("{0}/{1}", currentStr, initialStr);
        }
    }

    public void StartTimer()
    {
        _currentTime = initialTime;
        _isCounting = true;
        //UpdateTimerText();
    }

    private void ResetTimer()
    {
        _currentDelay = startDelay;
        _currentTime = initialTime;
        _isCounting = false;
        //UpdateTimerText();
    }
}
