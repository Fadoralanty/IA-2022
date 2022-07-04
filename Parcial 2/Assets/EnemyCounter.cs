using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemiesLeft;

    private void Awake()
    {
        _enemiesLeft = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _enemiesLeft.text = "Boxes Left: " + GameManager.instance.enemies.Count;
    }
}
