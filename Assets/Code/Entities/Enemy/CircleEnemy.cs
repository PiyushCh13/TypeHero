using TMPro;
using UnityEngine;

public class CircleEnemy : MonoBehaviour
{
    private GameObject player;
    public float enemySpeed;

    private float player_Distance;
    private RectTransform rectTransform;
    public TMP_Text wordText;
    private WordManager wordManager;

    void Start()
    {
        player = GameObject.Find("Player");
        wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        rectTransform = GetComponent<RectTransform>();
        wordText = GetComponentInChildren<TMP_Text>();
        SetWord(wordText);
    }


    void Update()
    {
        FollowPlayer();
        
        player_Distance = Vector3.Distance(player.transform.position, transform.position);

        if (player_Distance < 20f)
        {
            DeactivateEnemy(this.gameObject);
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = player.transform.position - rectTransform.position;
        rectTransform.Translate(direction.normalized * Time.deltaTime * enemySpeed);
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    private void SetWord(TMP_Text text)
    {
        text.text = wordManager.GetRandomWord();
    }
}
