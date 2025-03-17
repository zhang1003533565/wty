using UnityEngine;

public class EnemyBasicObj : MonoBehaviour, IInteractable
{
    public string enemyName = "Enemy";
    public int maxHealth = 100;
    public float healSpeed = 5f; // 每秒回血速度
    public float reviveTime = 5f; // 复活时间，负数表示不复活
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public GameObject healthBar; // UI 公告板血量展示槽

    [Range(0, 1)] public float dropProbability; // 掉落概率 (0 到 1)
    public GameObject dropPrefab; // 怪物的预制体

    private int currentHealth; 
    private float lastHitTime;
    private Vector3 initialPosition;
    private bool isDead;

    public void Drop()
    {
        if (Random.value <= dropProbability)
        {
            // 实例化掉落的物品
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
    }

    public bool isDeath()
    {
        return isDead;
    }

    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        lastHitTime = Time.time;
        isDead = false;

        if (healthBar != null)
        {
            UpdateHealthBar();
        }
    }

    void Update()
    {
        if (isDead) return;

        // 自动回血逻辑
        if (Time.time - lastHitTime > 3f && currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + (int)(healSpeed * Time.deltaTime));
            if (healthBar != null)
            {
                UpdateHealthBar();
            }
        }
    }

    public bool IsInteractable()
    {
        return false;
    }

    public bool IsAttackable()
    {
        return !isDead;
    }

    public string GetName()
    {
        return enemyName;
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + enemyName);
    }

    public void TakeDamage(string weaponName, int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        lastHitTime = Time.time;

        if (healthBar != null)
        {
            UpdateHealthBar();
        }

        // 播放被攻击动画和声音
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // 广播被攻击信息
        BroadcastMessage("OnTakeDamage", SendMessageOptions.DontRequireReceiver);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0;

        // 播放死亡动画和声音
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Drop();

        // 广播死亡信息
        BroadcastMessage("OnDie", SendMessageOptions.DontRequireReceiver);

        // 处理复活逻辑
        if (reviveTime >= 0)
        {
            Invoke("Revive", reviveTime);
        }
    }

    private void Revive()
    {
        isDead = false;
        currentHealth = maxHealth;
        transform.position = initialPosition;

        if (healthBar != null)
        {
            UpdateHealthBar();
        }

        // 播放复活动画（如果有）
        if (animator != null)
        {
            animator.SetTrigger("Live");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthRatio = (float)currentHealth / maxHealth;
            healthBar.transform.localScale = new Vector3(healthRatio, 1, 1);
        }
    }
}
