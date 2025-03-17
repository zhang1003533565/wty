using UnityEngine;

public class EnemyBasicObj : MonoBehaviour, IInteractable
{
    public string enemyName = "Enemy";
    public int maxHealth = 100;
    public float healSpeed = 5f; // ÿ���Ѫ�ٶ�
    public float reviveTime = 5f; // ����ʱ�䣬������ʾ������
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;
    public GameObject healthBar; // UI �����Ѫ��չʾ��

    [Range(0, 1)] public float dropProbability; // ������� (0 �� 1)
    public GameObject dropPrefab; // �����Ԥ����

    private int currentHealth; 
    private float lastHitTime;
    private Vector3 initialPosition;
    private bool isDead;

    public void Drop()
    {
        if (Random.value <= dropProbability)
        {
            // ʵ�����������Ʒ
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

        // �Զ���Ѫ�߼�
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

        // ���ű���������������
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // �㲥��������Ϣ
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

        // ������������������
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Drop();

        // �㲥������Ϣ
        BroadcastMessage("OnDie", SendMessageOptions.DontRequireReceiver);

        // �������߼�
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

        // ���Ÿ����������У�
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
