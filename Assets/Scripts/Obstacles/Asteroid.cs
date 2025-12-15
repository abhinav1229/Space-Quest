using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private FlashWhite flashWhite;
    private ObjectPooler destroyEffectPool;

    private int lives;
    private int maxLives;
    private int damage;

    [SerializeField] private Sprite[] sprites;


    private void OnEnable() {
        lives = maxLives;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        flashWhite = GetComponent<FlashWhite>();
        destroyEffectPool = GameObject.Find("Boom2Pool").GetComponent<ObjectPooler>();

        rigidbody2d = GetComponent<Rigidbody2D>();
        float pushX = Random.Range(-1f, 0);
        float pushY = Random.Range(-1f, 1f);
        rigidbody2d.linearVelocity = new Vector2(pushX, pushY);

        float randomScale = Random.Range(0.6f, 1.0f);
        transform.localScale = new Vector2(randomScale, randomScale);

        maxLives = 5;
        lives = maxLives;
        damage = 1;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player)
            {
                player.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.HitRock);
        lives -= damage;
        flashWhite.Flash();
        if (lives <= 0)
        {
            GameObject destroyEffect = destroyEffectPool.GetPooledObject();
            destroyEffect.transform.SetPositionAndRotation(transform.position, transform.rotation);
            destroyEffect.SetActive(true);

            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.Boom2);
            flashWhite.Reset();
            gameObject.SetActive(false);
        }
    }
}
