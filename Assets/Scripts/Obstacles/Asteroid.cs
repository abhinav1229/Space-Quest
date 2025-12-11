using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;

    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private int lives;

    [SerializeField] private Sprite[] sprites;    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        defaultMaterial = spriteRenderer.material;

        rigidbody2d = GetComponent<Rigidbody2D>();
        float pushX = Random.Range(-1f, 0);
        float pushY = Random.Range(-1f, 1f);
        rigidbody2d.linearVelocity = new Vector2(pushX, pushY);

        float randomScale = Random.Range(0.6f, 1.0f);
        transform.localScale = new Vector2(randomScale, randomScale);
    }

    void Update()
    {
        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);

        if(transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {
            spriteRenderer.material = whiteMaterial;
            StartCoroutine(ResetMaterial());

            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.HitRock);

            lives -= 1;
            if(lives <= 0)
            {
                Instantiate(destroyEffect, transform.position, transform.rotation);
                AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.Boom2);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }
}
