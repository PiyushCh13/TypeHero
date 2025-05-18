using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WordManager : MonoBehaviour
{
    public List<string> wordsInsideMemory = new List<string>();
    private string wordfilePath;

    void Start()
    {
        wordfilePath = Application.streamingAssetsPath + "/words.txt";
        StartCoroutine(LoadWordsinList());
    }

    private IEnumerator LoadWordsinList()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(wordfilePath);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load words: " + webRequest.error);
        }
        else
        {
            string content = webRequest.downloadHandler.text;
            string[] lines = content.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            wordsInsideMemory = new List<string>(lines);
        }
    }

    public string GetRandomWord()
    {
        if (wordsInsideMemory.Count == 0)
        {
            Debug.LogWarning("Word list is empty.");
            return "";
        }
        int randomIndex = Random.Range(0, wordsInsideMemory.Count);
        return wordsInsideMemory[randomIndex];
    }
}
