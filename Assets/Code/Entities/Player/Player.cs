using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<CircleEnemy> circleEnemiesinScene;
    public List<string> wordsinScene;
    public string keyPressed;

    void Start()
    {
        wordsinScene = new List<string>();
    }

    void Update()
    {
        circleEnemiesinScene = new List<CircleEnemy>(FindObjectsByType<CircleEnemy>(FindObjectsSortMode.None));
        UpdateWordsInScene();

        if (Input.anyKeyDown)
        {
            keyPressed += Input.inputString;
            if (!string.IsNullOrEmpty(keyPressed)) // Ignore empty inputs
            {
                CheckWord(keyPressed);
            }
        }
    }

    private void UpdateWordsInScene()
    {
        wordsinScene.Clear();

        foreach (var enemy in circleEnemiesinScene)
        {
            if (enemy.wordText != null)
            {
                wordsinScene.Add(enemy.wordText.text);
            }
        }
    }

    private void CheckWord(string word)
    {
        if (wordsinScene.Contains(word))
        {
            foreach (var enemy in circleEnemiesinScene)
            {
                if (enemy.wordText.text == word)
                {
                    enemy.DeactivateEnemy(enemy.gameObject);
                }
            }
        }
    }
}
