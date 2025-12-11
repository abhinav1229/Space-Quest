using UnityEngine;

public class PhaserWeapon : MonoBehaviour
{
    public static PhaserWeapon Instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private ObjectPooler bulletPool;

    public float speed;
    public int damage;

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

    public void Shoot()
    {
        GameObject bullet = bulletPool.GetPooledObject();
        bullet.transform.position = transform.position;
        bullet.SetActive(true);

        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.Shoot);
    }
}
