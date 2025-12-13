using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private FlashWhite flashWhite;
    private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;

    public bool boosting = false;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private ParticleSystem boostParticle;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashWhite = GetComponent<FlashWhite>();

        energy = maxEnergy;
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);

        health = maxHealth;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            float directionX = Input.GetAxisRaw("Horizontal");
            float directionY = Input.GetAxisRaw("Vertical");
            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", directionY);

            playerDirection = new Vector2(directionX, directionY).normalized;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
            {
                EnterBoost();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))
            {
                ExitBoost();
            }

            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetButtonDown("Fire1"))
            {
                PhaserWeapon.Instance.Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed, playerDirection.y * moveSpeed);

        if (boosting)
        {
            if (energy >= 0.5f) energy -= 0.5f;
            else
            {
                ExitBoost();
            }
        }
        else
        {
            if (energy < maxEnergy)
            {
                energy += energyRegen;
            }
        }
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
    }

    private void EnterBoost()
    {
        if (energy > 10)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.Fire);
            animator.SetBool("boosting", true);
            GameManager.Instance.SetWorldSpeed(7.0f);
            boosting = true;
            boostParticle.Play();
        }
    }

    public void ExitBoost()
    {
        animator.SetBool("boosting", false);
        GameManager.Instance.SetWorldSpeed(1.0f);
        boosting = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid)
            {
                asteroid.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        AudioManager.Instance.PlaySound(AudioManager.Instance.Hit);
        flashWhite.Flash();
        if (health <= 0)
        {
            GameManager.Instance.SetWorldSpeed(0f);
            gameObject.SetActive(false);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            GameManager.Instance.GameOver();
            AudioManager.Instance.PlaySound(AudioManager.Instance.Ice);
        }
    }
}
