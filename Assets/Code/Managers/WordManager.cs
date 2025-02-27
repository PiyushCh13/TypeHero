using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{

    public List<string> wordsInsideMemory;

    private string wordfilePath = Application.streamingAssetsPath + "/words.txt";

    void Start()
    {
        LoadWordsinList();
    }

    void Update()
    {
        
    }

    private void LoadWordsinList()
    {
        if (CheckPath(wordfilePath))
        {
            string[] lines = System.IO.File.ReadAllLines(wordfilePath);
            wordsInsideMemory = new List<string>(lines);
        }
    }

    private bool CheckPath(string path)
    {
        return System.IO.File.Exists(path);
    }

    public string GetRandomWord()
    {
        int randomIndex = Random.Range(0, wordsInsideMemory.Count);
        return wordsInsideMemory[randomIndex];
    }
}
