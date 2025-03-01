using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    private int poolSize = 50;
    private GameObject[] pool;

    [Header("Enemy List")]
    private HashSet<GameObject> inactiveEnemies = new HashSet<GameObject>();
    private HashSet<GameObject> activeEnemies = new HashSet<GameObject>();

    [SerializeField] public List<GameObject> activeEnemiesList;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float objectDistance;


    void Start()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }

        InvokeRepeating("GetEnemy", 0, 5);
    }

    void Update()
    {
        activeEnemiesList = activeEnemies.ToList();
    }

    private GameObject GetEnemy()
    {
        inactiveEnemies.Clear();
        activeEnemies.Clear();

        foreach (GameObject enemy in pool)
        {
            if (!enemy.activeInHierarchy)
            {
                inactiveEnemies.Add(enemy);
            }
            else
            {
                activeEnemies.Add(enemy);
            }
        }

        if (inactiveEnemies.Count > 0)
        {
            GameObject selectedEnemy = inactiveEnemies.ElementAt(Random.Range(0, inactiveEnemies.Count));
            selectedEnemy.transform.position = CalculatePosition();
            selectedEnemy.SetActive(true);
            return selectedEnemy;
        }

        return null;
    }

    private Vector3 CalculatePosition()
    {
        float angle = Random.Range(0, 360);
        float radianAngle = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radianAngle) * objectDistance;
        float y = Mathf.Sin(radianAngle) * objectDistance;
        return new Vector3(x, y, 0);
    }

}
