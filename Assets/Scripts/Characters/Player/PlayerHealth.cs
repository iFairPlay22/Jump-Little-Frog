using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
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
        ReduceHealth(1);
    }

    public void ReduceHealth(int damages)
    {
        _livesRemaining = Mathf.Max(_livesRemaining - damages, 0);
        _UpdateLifeUI();
    }

    public void IncreaseHealth()
    {
        _livesRemaining = Mathf.Min(_livesRemaining + 1, _maxLives);
        _UpdateLifeUI();
    }

    public void DestroyHealth()
    {
        _livesRemaining = 0;
        _UpdateLifeUI();
    }

    public void RestartHealth()
    {
        _livesRemaining = _maxLives;
        _UpdateLifeUI();
    }

    void _UpdateLifeUI()
    {
        if (_livesRemaining == 0)
        {
            FindObjectOfType<Player>().Die();
        }

        LifeSlicedImage.fillAmount = ((float)_livesRemaining) / ((float)_maxLives);
    }
}
