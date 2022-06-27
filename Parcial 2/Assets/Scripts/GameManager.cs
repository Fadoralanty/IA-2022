using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController Player;
    public List<GameObject> enemies;
    private bool _isGameFinished;
    [Header("Score")]
    public Action<float> OnScoreChange;
    public float currentScore = 0f;
    public float scoreMultiplier = 1f;
    public Action<float> OnMultiplierChange;
    
    [Header("Game_Time")]
    public float currentGameTime=0f;
    public float MaxLevelGameTime = 9999f;
    public UnityEvent OnLevelReset = new UnityEvent();
    public string NextLevel;
        
    [SerializeField] private GameObject HUD;    
    [SerializeField] private GameObject GameOverScreen;     
    [SerializeField] private GameObject Victory_Screen;
    
    public string currentLevel;

    public void Start()
    {
        currentGameTime = MaxLevelGameTime;
        currentLevel = SceneManager.GetActiveScene().name;
        OnScoreChange?.Invoke(currentScore);
        OnMultiplierChange?.Invoke(scoreMultiplier);
    }
    
    private void Awake()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        currentGameTime -= Time.deltaTime;
        if (currentGameTime <= 0)
        {
            GameOver();
        }

        if (enemies.Count == 0)
        {
            GameCompleted();
        }
        
    }

    public void GameOver()
    {
        _isGameFinished = true;
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        if (_isGameFinished)
        {
            Instantiate(GameOverScreen);
            _isGameFinished = false;
        }
        HUD.SetActive(false);
        Player.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    public void GameCompleted() //llamar al matar a un boss
    {
        _isGameFinished = true;
        StartCoroutine(LoadNextLevel(NextLevel));
    }
    IEnumerator LoadNextLevel(string levelName)
    {
        if (_isGameFinished)
        {
            Instantiate(Victory_Screen);
            _isGameFinished = false;
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(levelName);
        currentLevel = levelName;
        ResetLevelValues();
    }
    public void AddScore(float points)
    {
        currentScore += points * scoreMultiplier;
        OnScoreChange?.Invoke(currentScore);
    }    
    public void AddMultiplier(float multiplier)
    {
        scoreMultiplier += multiplier ;
        OnMultiplierChange?.Invoke(scoreMultiplier);
    }

    public void ResetMultiplier()
    {
        scoreMultiplier = 1;
        OnMultiplierChange?.Invoke(scoreMultiplier);
    }
    
    public void SubstractScore(float points)
    {
        currentScore -= points;
        OnScoreChange?.Invoke(currentScore);
    }
    public void ResetLevelValues()
    {
        currentGameTime = 0f;
        currentScore = 0f;
        scoreMultiplier = 1f;
        OnScoreChange?.Invoke(currentScore);
        OnLevelReset.Invoke();
    }


}
