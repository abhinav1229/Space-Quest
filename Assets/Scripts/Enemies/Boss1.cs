using System.Data;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    private Animator animator;

    private float speedX;
    private float speedY;
    private bool charging;

    private float switchInterval;
    private float switchTimer;

    private int lives;

    void Start()
    {
        lives = 100;
        animator = transform.GetComponent<Animator>();
        EnterChargeState();
    }

    void Update()
    {
        if (switchTimer > 0)
        {
            switchTimer -= Time.deltaTime;
        }
        else
        {
            if (charging)
            {
                EnterPatrolState();
            }
            else
            {
                EnterChargeState();
            }
        }

        if (transform.position.y > 3 || transform.position.y < -3)
        {
            speedY *= -1;
        }

        float moveX = Time.deltaTime;
        float moveY = speedY * Time.deltaTime;

        transform.position += new Vector3(-moveX, moveY);

        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }

    private void EnterPatrolState()
    {
        speedX = 0;
        speedY = Random.Range(-2f, 2f);
        switchInterval = Random.Range(5f, 10f);
        switchTimer = switchInterval;
        charging = false;
        animator.SetBool("charging", false);
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.BossCharge);
    }

    private void EnterChargeState()
    {
        speedX = -5f;
        speedY = 0;
        switchInterval = Random.Range(2f, 2.5f);
        switchTimer = switchInterval;
        charging = true;
        animator.SetBool("charging", true);
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.BossCharge);
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.HitArmor);
        lives -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(0);
        }
    }
}
