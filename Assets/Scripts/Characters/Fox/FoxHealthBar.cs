using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxHealthBar : MonoBehaviour
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

        if (_livesRemaining == 0)
        {
            FindObjectOfType<Fox>().Die();
        }
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
        LifeSlicedImage.fillAmount = ((float)_livesRemaining) / ((float)_maxLives);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            ReduceHealth();
    }
}
