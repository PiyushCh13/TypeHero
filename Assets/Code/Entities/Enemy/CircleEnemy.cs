using TMPro;
using UnityEngine;

public class CircleEnemy : MonoBehaviour
{
    [Header("Player")]
    private Player player;
    private float player_Distance;

    [Header("Enemy Attributes")]
    public float enemySpeed;
    private float baseSpeed = 2;
    public float distanceBeforeDeactivation;

    [Header("Enemy Components")]
    private RectTransform rectTransform;

    [Header("Managers")]
    private WordManager wordManager;
    private AudioManager audioManager;
    [SerializeField] private EnemyPoolManager enemyPoolManager;

    [Header("Enemy Text")]
    public TMP_Text wordText;
    [SerializeField] private Color32 defaultTextColor;

    public string[] midGameMessages;

    [Header("VFX")]
    public GameObject explosionVFX;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemyPoolManager = GameObject.Find("Enemy").GetComponent<EnemyPoolManager>();
        wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        rectTransform = GetComponent<RectTransform>();
        wordText = GetComponentInChildren<TMP_Text>();
        wordText.color = defaultTextColor;
        SetWord(wordText);
    }


    void Update()
    {
        FollowPlayer();
        enemySpeed = baseSpeed + (Time.timeSinceLevelLoad * 0.1f);


        player_Distance = Vector3.Distance(player.transform.position, transform.position);

        if (player_Distance <= distanceBeforeDeactivation)
        {
            DeactivateEnemy();
            player.TakeDamage(10);
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        transform.Translate(direction.normalized * Time.deltaTime * enemySpeed);
    }

    public void DeactivateEnemy()
    {
        enemyPoolManager.UpdateList();
        player.StartCoroutine(player.Shake(0.5f, 0.5f));
        Explosion(this.gameObject);
        audioManager.PlaySFX();
        this.gameObject.SetActive(false);
        player.currentEnemy = null;
        player.currentKeyword = "";
        player.currentKey = '\0';
        player.currentText.text = midGameMessages[Random.Range(0, midGameMessages.Length)];
        player.coloredIndices.Clear();
        SetWord(wordText);
    }

    private void Explosion(GameObject enemy)
    {
        GameObject explosion = Instantiate(explosionVFX, enemy.transform.position, Quaternion.identity);
        ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Destroy(explosion, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        }
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
