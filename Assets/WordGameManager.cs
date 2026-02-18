using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using UnityEditor;

public class WordGameManager : MonoBehaviour
{
    // Variables 
    [SerializeField] TMP_Text livesPrompt;
    [SerializeField] TMP_Text mainPrompt;
    [SerializeField] TMP_Text lettersGuessed;
    [SerializeField] TMP_Text correctLetters;
    [SerializeField] TMP_InputField charInput;
    [SerializeField] TextAsset wordList;
    List<char> wordAns = new List<char>();
    string[] words;
    List<char> guess = new List<char>();
    int numLives;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        words = wordList.text.Split('\n');

        foreach (string s in words)
        {
            s.Trim();
        }

        GameStart();
    }

    // Update is called once per frame
    void Update() 
    {
        // Constantly checks if there are no lives left
        if (numLives == 0)
        {
            GameOver();
            numLives = -1;
        }
    }

    // GameStart is called at the start and whenever the Reset Button is pressed
    public void GameStart()
    {
        wordAns = new List<char>();
        guess = new List<char>();
        numLives = 3;
        livesPrompt.text = "Lives: 3";
        mainPrompt.text = "Guess a Letter";
        lettersGuessed.text = "Letters Guessed: ";

        // Sets the word answer and sets the list in guess to all blanks of the same amount of letters
        int randomNum = UnityEngine.Random.Range(0, words.Length);
        foreach (char c in words[randomNum].ToUpper())
            wordAns.Add(c);
        for (int i = 0; i < wordAns.Count - 1; i++)
            guess.Add('_');
        UpdateCorrectLetters();
    }
    
    // GameOver is called when there are 0 lives left
    void GameOver()
    {
        livesPrompt.text = "Lives: 0";
        mainPrompt.text = "No lives left. You lose.";
        guess = wordAns;
        UpdateCorrectLetters();
    }

    // SubmitButton is called whenever the player submits an answer
    public void SubmitButton()
    {
        // As long as numLives is greater than 0, the Submit Button will do something, otherwise do nothing.
        if (numLives > 0)
        {
            char userInput = char.Parse(charInput.text.ToUpper());

            // If wordAns list has the character from userInput, it is correct and put into guess list
            if (wordAns.Contains(userInput))
            {
                mainPrompt.text = "Correct! Guess again";
                for (int i = 0; i < wordAns.Count; i++)
                {
                    if (wordAns[i] == userInput)
                    {
                        guess[i] = wordAns[i];
                    }
                }

                UpdateCorrectLetters();
            }
            else if (lettersGuessed.text.Substring(15).Contains(userInput))
            {
                mainPrompt.text = "Already guessed that letter";
            }
            else
            {
                numLives--;
                livesPrompt.text = "Lives: " + numLives;
                mainPrompt.text = "Wrong letter. Guess again";
                lettersGuessed.text += (userInput + " ");
            }
            if (!guess.Contains('_'))
            {
                numLives = -1;
                mainPrompt.text = "You won! Play again?";
            }
        }
    }

    // UpdateCorrectLetters is called whenever a change to the displayed correct letters has been made
    void UpdateCorrectLetters()
    {
        correctLetters.text = "";
        foreach (char c in guess)
            correctLetters.text += (c + " ");
        correctLetters.text = correctLetters.text.Substring(0, correctLetters.text.Length - 1);
    }
}
