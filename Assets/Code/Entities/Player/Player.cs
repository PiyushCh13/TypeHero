using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Enemy Components")]
    public CircleEnemy currentEnemy;
    [SerializeField] private EnemyPoolManager enemyPoolManager;

    [Header("Player Components")]
    public char currentKey;
    public string currentKeyword = "";
    public Color32 typedTextColor;

    private int health = 100;
    private int killCounter = 0;

    [Header("UI Components")]
    public Slider healthSlider;
    public TMP_Text currentText;
    public TMP_Text killCount;

    [Header("Gameplay Components")]
    public UIManager uIManager;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                currentKey = Input.inputString[0];
                FindCurrentEnemy(currentKey);
                ChangeWordColor(currentKey);
            }
        }
    }

    void FindCurrentEnemy(char currentKey)
    {
        if (currentEnemy == null)
        {
            foreach (GameObject enemy in enemyPoolManager.activeEnemiesList)
            {
                CircleEnemy circleEnemy = enemy.GetComponent<CircleEnemy>();

                if (circleEnemy != null)
                {
                    if (circleEnemy.wordText.text[0] == currentKey)
                    {
                        currentEnemy = circleEnemy;
                        currentText.text = currentEnemy.wordText.text;
                    }
                }
            }
        }
    }

    void ChangeWordColor(char letter)
    {
        if (currentEnemy == null || currentEnemy.wordText == null)
            return;

        TMP_Text textMeshPro = currentEnemy.wordText;
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        textMeshPro.ForceMeshUpdate();

        bool letterAdded = false;
        Color32[] newVertexColors = new Color32[textInfo.meshInfo[0].colors32.Length];

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            newVertexColors = textInfo.meshInfo[i].colors32.ToArray();
        }

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) continue;

            if (charInfo.character == letter && !letterAdded && currentKeyword.Length < currentEnemy.wordText.text.Length)
            {
                int nextIndex = currentKeyword.Length;

                if (i == nextIndex)
                {
                    currentKeyword += letter;
                    letterAdded = true;

                    int vertexIndex = charInfo.vertexIndex;

                    for (int j = 0; j < 4; j++)
                    {
                        newVertexColors[vertexIndex + j] = typedTextColor;  // Change color and store
                    }
                }
            }
        }

        // Apply the stored colors
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].colors32 = newVertexColors;
        }

        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        if (currentKeyword == currentEnemy.wordText.text)
        {
            currentEnemy.DeactivateEnemy(currentEnemy.gameObject);
            IncreaseKillCount();
        }
    }

    public void UpdateUI()
    {
        healthSlider.value = health;
        killCount.text = "KILLCOUNT : " + killCounter.ToString();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateUI();

        if(health <= 0)
        {
            uIManager.GameOver();
        }
    }

    public void IncreaseKillCount()
    {
        killCounter++;
        UpdateUI();
    }
}