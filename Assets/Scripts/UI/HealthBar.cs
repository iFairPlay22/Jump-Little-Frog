using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Image LifeSlicedImage;

    [SerializeField]
    int _maxLives;

    int _livesRemaining;

    void Start()
    {
        RestartHealth();
    }

    public void ReduceHealth()
    {
        _livesRemaining = Mathf.Max(_livesRemaining - 1, 0);
        _UpdateLifeUI();
    }

    public void IncreaseHealth()
    {
        _livesRemaining = Mathf.Min(_livesRemaining + 1, _maxLives);
        _UpdateLifeUI();
    }

    public void RestartHealth()
    {
        _livesRemaining = _maxLives;
        _UpdateLifeUI();
    }

    void _UpdateLifeUI()
    {
        LifeSlicedImage.fillAmount = ((float)_livesRemaining) / ((float)_maxLives);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            ReduceHealth();
    }
}
