using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameTime;

    private void Awake()
    {
        _gameTime = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        
        _gameTime.text = GameManager.instance.currentGameTime.ToString("0");
    }
}
