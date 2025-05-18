using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
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

    private int health = 75;
    private int killCounter = 0;

    [Header("UI Components")]
    public Slider healthSlider;
    public TMP_Text currentText;
    public TMP_Text killCount;
    public HashSet<int> coloredIndices = new HashSet<int>();

    [Header("Gameplay Components")]
    public UIManager uIManager;

    void Start()
    {
        UpdateUI();
        currentText.text = uIManager.GetWittyWords();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!string.IsNullOrEmpty(Input.inputString))
            {
                currentKey = Input.inputString[0];
                CheckForNearestEnemy(currentKey);
                ChangeWordColor(currentKey);
            }
        }
    }

    public void CheckForNearestEnemy(char key)
    {
        if (currentEnemy == null || !currentEnemy.gameObject.activeInHierarchy)
        {
            CircleEnemy nearestEnemy = null;
            float closestDistance = float.MaxValue;

            Vector3 playerWorldPos = transform.position;

            foreach (GameObject enemy in enemyPoolManager.activeEnemiesList.ToList())
            {
                // if (enemy == null || !enemy.activeInHierarchy)
                // {
                //     enemyPoolManager.activeEnemiesList.Remove(enemy);
                //     continue;
                // }

                CircleEnemy circleEnemy = enemy.GetComponent<CircleEnemy>();

                if (circleEnemy.wordText.text[0] == key)
                {
                    Vector3 enemyWorldPos = circleEnemy.transform.position;
                    float distance = Vector3.Distance(playerWorldPos, enemyWorldPos);

                    if (distance < closestDistance || (distance == closestDistance && circleEnemy.GetInstanceID() < nearestEnemy.GetInstanceID()))
                    {
                        closestDistance = distance;
                        nearestEnemy = circleEnemy;
                    }

                }
            }

            if (nearestEnemy != null)
            {
                currentEnemy = nearestEnemy;
                currentText.text = currentEnemy.wordText.text;
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

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            if (coloredIndices.Contains(i))
            {
                for (int j = 0; j < 4; j++)
                {
                    vertexColors[vertexIndex + j] = typedTextColor;
                }
            }

            if (charInfo.character == letter && !letterAdded && currentKeyword.Length == i)
            {
                currentKeyword += letter;
                letterAdded = true;
                coloredIndices.Add(i);

                for (int j = 0; j < 4; j++)
                {
                    vertexColors[vertexIndex + j] = typedTextColor;
                }
            }
        }

        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        if (currentKeyword == currentEnemy.wordText.text)
        {
            currentEnemy.DeactivateEnemy();
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

        if (health <= 0)
        {
            uIManager.GameOver();
        }
    }

    public void IncreaseKillCount()
    {
        killCounter++;
        UpdateUI();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = Camera.main.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPosition;
    }
}