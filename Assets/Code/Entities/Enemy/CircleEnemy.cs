using TMPro;
using UnityEngine;

public class CircleEnemy : MonoBehaviour
{
    [Header("Player")]
    private GameObject player;
    private float player_Distance;

    [Header("Enemy Attributes")]
    public float enemySpeed;
    public float distanceBeforeDeactivation;

    [Header("Enemy Components")]
    private RectTransform rectTransform;

    [Header("Managers")]
    private WordManager wordManager;
    private AudioManager audioManager;

    [Header("Enemy Text")]
    public TMP_Text wordText;
    [SerializeField] private Color32 defaultTextColor;

    [Header("VFX")]
    public GameObject explosionVFX;

    void Start()
    {
        player = GameObject.Find("Player");
        wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        rectTransform = GetComponent<RectTransform>();
        wordText = GetComponentInChildren<TMP_Text>();
        wordText.color = defaultTextColor;
        SetWord(wordText);
    }


    void Update()
    {
        FollowPlayer();
        
        player_Distance = Vector3.Distance(player.transform.position, transform.position);

        if (player_Distance < distanceBeforeDeactivation)
        {
            DeactivateEnemy(this.gameObject);
            player.GetComponent<Player>().TakeDamage(10);
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = player.transform.position - rectTransform.position;
        rectTransform.Translate(direction.normalized * Time.deltaTime * enemySpeed);
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        Instantiate(explosionVFX, enemy.transform.position, Quaternion.identity);
        enemy.SetActive(false);
        SetWord(wordText);
        player.GetComponent<Player>().currentKeyword = "";
        player.GetComponent<Player>().currentEnemy = null;

    }

    private void SetWord(TMP_Text text)
    {
        text.text = wordManager.GetRandomWord();
    }

    public void SetSpeed(float speed)
    {
        enemySpeed = speed;
    }
}
