using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public float CurrentLife => _currentLife;
    [SerializeField] protected float _currentLife;

    public float Maxlife => _maxLife;
    [SerializeField] protected float _maxLife;
    
    public bool IsDead => _isDead;
    private bool _isDead = false;
    
    public bool IsInvulnerable => _isInvulnerable; //Quiero que el player tenga unos segundos de inmortalidad cuando le disparen y esquive
    [SerializeField] private bool _isInvulnerable;

    private float _invulnerableTime;
    private float _currentInvulnerableTime;

    //events
    public Action<float> OnLifeUpdate;
    public UnityEvent OnDie = new UnityEvent();
    
    protected virtual void Awake()
    {
        InitStats();
    }
    private void InitStats() //privado porque solo se deveria llamar en Awake, por eso el nombre Initialize Stats
    {
        _isDead = false;
        _isInvulnerable = false;
        _currentLife = _maxLife;
        OnLifeUpdate?.Invoke(_currentLife);
    }

    private void Update()
    {
        if (_isInvulnerable)
        {
            _currentInvulnerableTime += Time.deltaTime;
            if (_currentInvulnerableTime >= _invulnerableTime)
            {
                _isInvulnerable = false;
                _currentInvulnerableTime = 0f;
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if(_isInvulnerable) return;
        if (_isDead) return;
        
        _currentLife -= damage;
        if(_currentLife <= 0)
        {
            _currentLife = 0;
            Die();
        }
        OnLifeUpdate?.Invoke(_currentLife);
    }
    public virtual void AddLife(float HP)
    {
        if(_isDead) return;
        
        _currentLife += HP;
        if (_currentLife >= _maxLife)
            _currentLife = _maxLife;
        OnLifeUpdate?.Invoke(_currentLife);
    }

    public virtual void SetInvulnerable(float time)
    {
        if(_isDead) return;
        
        _invulnerableTime = time;
        _isInvulnerable = true;
        _currentInvulnerableTime = 0f;
    }

    public void Die()
    {
        _isDead = true;
        OnDie?.Invoke();
    }

    public void ResetValues()
    {
        InitStats();   
    }
    
}
