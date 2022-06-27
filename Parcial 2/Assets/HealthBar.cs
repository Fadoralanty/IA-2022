using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Main HealthBar")]
    [SerializeField] private Image _healthBar;

    [Header("Sub HealhBar")]
    [SerializeField] private Image _healthBarDelay;
    [SerializeField] private float _loseHealthSpeed= 0.2f;
    [SerializeField] private float _loseHPSpeedNormal= 0.2f;
    [SerializeField] private float _loseHealthSpeedOneShot= 2f;

    [Header("Life Components")]
    public Damageable _damageable;


    private void Awake()
    {
        _damageable.OnLifeUpdate += FillHealthbar;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Fill2ndHealthbar();
    }
    public void FillHealthbar(float currentlife)
    {
        _healthBar.fillAmount = currentlife/_damageable.Maxlife;
        _loseHealthSpeed = _loseHPSpeedNormal;
    }   
    public void Fill2ndHealthbar()
    {
        if (_healthBar.fillAmount == 0)
        {
            _loseHealthSpeed = _loseHealthSpeedOneShot;
        }
        if (_healthBarDelay.fillAmount>_healthBar.fillAmount)
        {
            _healthBarDelay.fillAmount -= _loseHealthSpeed  * Time.deltaTime;
        }
        else
        {
            _healthBarDelay.fillAmount = _healthBar.fillAmount;
        }
    }

}
