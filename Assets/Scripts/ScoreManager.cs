using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : BaseUI
{
    private static ScoreManager _instance;

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>();
            }
            return _instance;
        }
    }

    private int currentFrame = 1;
    private int rollCount = 0; 
    private int[] frameScores = new int[10]; 
    private int[] rolls = new int[21]; 
    private int fallenPinCount = 0; 

    public Button startButton; 
    public Button nextBowlingButton; 
    public TextMeshProUGUI finalScoreText;

    public PinGroup pinGroup;
    public PinsScore pinsScore;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Bind();
        nextBowlingButton.interactable = false;
        startButton.onClick.AddListener(StartGame);
        nextBowlingButton.onClick.AddListener(NextBowling);
    }

    private void Start()
    {
        for (int i = 0; i < frameScores.Length; i++)
        {
            frameScores[i] = 0;
        }
    }

    public void StartGame()
    {
        currentFrame = 1; 
        rollCount = 0; 
        fallenPinCount = 0; 
        ActivateFrame(currentFrame);
        nextBowlingButton.interactable = true; 
        finalScoreText.text = "";
        startButton.interactable = false;
    }

    private void ActivateFrame(int frame)
    {
        Image frameImage = GetUI<Image>($"{frame}Frame");
        frameImage.color = Color.yellow;
    }

    public void NotifyPinFallen(int pins)
    {
        fallenPinCount = pins;
        RecordScore(fallenPinCount);
    }

    public void NextBowling()
    {
        if (currentFrame > 10)
        {
            CalculateFinalScore();
            return;
        }

        rollCount++;

        if (rollCount >= 2)
        {
            rollCount = 0;
            currentFrame++;
            if (currentFrame <= 10)
            {
                ActivateFrame(currentFrame);
                pinGroup.ResetPositions();
                pinsScore.ResetPinUI();
            }
        }


        fallenPinCount = 0;
    }


    private void CalculateFinalScore()
    {
        int totalScore=frameScores[frameScores.Length - 1];
        finalScoreText.text = $"{totalScore}"; 
    }


    private void RecordScore(int pins)
    {
        if (rollCount == 0)
        {
            rolls[(currentFrame - 1) * 2] += pins;
            UpdateScoreUI($"{currentFrame}-1 Score", rolls[(currentFrame - 1) * 2]);

            if (pins == 10) 
            {
                Strike();
                UpdateScoreUI($"{currentFrame}-2 Score", "/");
                rollCount = 2;
                frameScores[currentFrame - 1] = frameScores[currentFrame - 2] + 10;
                UpdateScoreUI($"{currentFrame} Score", frameScores[currentFrame - 1]);
            }
        }
        else if (rollCount == 1) 
        {
            rolls[(currentFrame - 1) * 2 + 1] += pins - rolls[(currentFrame - 1) * 2];
            UpdateScoreUI($"{currentFrame}-2 Score", rolls[(currentFrame - 1) * 2 + 1]);
            if (pins == 10)
            {
                Spare();
            }

            if (currentFrame > 1)
            {
                if (rolls[(currentFrame - 2) * 2] == 10)
                {
                    frameScores[currentFrame - 2] += rolls[(currentFrame - 1) * 2] + rolls[(currentFrame - 1) * 2 + 1];
                    UpdateScoreUI($"{currentFrame - 1} Score", frameScores[currentFrame - 2]);
                }
                else if (rolls[(currentFrame - 2) * 2] + rolls[(currentFrame - 2) * 2 + 1] == 10)
                {
                    frameScores[currentFrame - 2] += rolls[(currentFrame - 1) * 2];
                    UpdateScoreUI($"{currentFrame - 1} Score", frameScores[currentFrame - 2]);
                }
            }



            if (currentFrame > 1)
            {
                frameScores[currentFrame - 1] = rolls[(currentFrame - 1) * 2] + rolls[(currentFrame - 1) * 2 + 1] + frameScores[currentFrame - 2];
            }
            else
            {
                frameScores[currentFrame - 1] = rolls[(currentFrame - 1) * 2] + rolls[(currentFrame - 1) * 2 + 1];
            }

            UpdateScoreUI($"{currentFrame} Score", frameScores[currentFrame - 1]);
        }
    }

    private void Spare()
    {
        Debug.Log("스페어");
    }

    private void Strike()
    {
        Debug.Log("스트라이크");
    }


    private void UpdateScoreUI(string scoreKey, object score)
    {
        GetUI<TextMeshProUGUI>(scoreKey).text = score.ToString();
    }

    public void ResetGame()
    {
        currentFrame = 1;
        rollCount = 0;
        fallenPinCount = 0;

        for (int i = 0; i < frameScores.Length; i++)
        {
            frameScores[i] = 0;
        }

        for (int i = 0; i < rolls.Length; i++)
        {
            rolls[i] = 0;
        }

        finalScoreText.text = "";
        startButton.interactable = true;
        nextBowlingButton.interactable = false;

        for (int i = 1; i <= 10; i++)
        {
            Image frameImage = GetUI<Image>($"{i}Frame");
            frameImage.color = new Color(0f, 0f, 0f, 0.20f);

            GetUI<TextMeshProUGUI>($"{i} Score").text = "";
            GetUI<TextMeshProUGUI>($"{i}-1 Score").text = "";
            GetUI<TextMeshProUGUI>($"{i}-2 Score").text = "";

        }
    }



}
