using System.Security.Cryptography;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float objectDistance;
    private int poolSize = 50;
    private GameObject[] pool;


    void Start()
    {
        pool = new GameObject[poolSize];
        for(int i = 0; i < poolSize; i++)
        {         
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }

        InvokeRepeating("GetEnemy", 0, 3);
    }

    private GameObject GetEnemy()
    {
        foreach(GameObject enemy in pool)
        {
            if(!enemy.activeInHierarchy)
            {
                enemy.transform.position = CalculatePosition();
                enemy.SetActive(true);
                return enemy;
            }
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
