using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // ����ʵ��
    public static PlayerState Instance { get; private set; }

    // ���״̬����
    public float Health;
    public float Hunger;
    public float Thirst;
    public float Stamina;

    // ��Ȼ�����ٶ�
    public float HealthRegenRate=0.2f;
    public float HungerDecayRate=-0.05f;
    public float ThirstDecayRate=-0.05f;
    public float StaminaRegenRate = 5f;

    // �ⲿ�ı��ʼ��Ȼ�������ӳ�ʱ��
    public float HealthRegenDelay = 5f;
    public float HungerDecayDelay = 30f;
    public float ThirstDecayDelay = 30f;
    public float StaminaRegenDelay = 0.5f;

    // ����ֵ�۳��ٶȣ���������ڿ�Ϊ0ʱ��
    public float HealthDecayRate=0.5f;

    // ���������־
    public bool IsDead;

    // ��ʱ��
    private float healthRegenTimer;
    private float hungerDecayTimer;
    private float thirstDecayTimer;
    private float staminaRegenTimer;

    private Transform PlayerNode; // ��ҽڵ�
    private Vector3 SpawnPos;
    private Quaternion SpawnRot;

    private void Awake()
    {
        // ����ģʽ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ��ʼ��״̬
        ResetState();
    }

    private void Update()
    {
        if (IsDead) return;

        // ���¼�ʱ��
        healthRegenTimer -= Time.deltaTime;
        hungerDecayTimer -= Time.deltaTime;
        thirstDecayTimer -= Time.deltaTime;
        staminaRegenTimer -= Time.deltaTime;

        // ��Ȼ������˥��
        if (healthRegenTimer <= 0) Health += HealthRegenRate * Time.deltaTime;
        if (hungerDecayTimer <= 0) Hunger += HungerDecayRate * Time.deltaTime;
        if (thirstDecayTimer <= 0) Thirst += ThirstDecayRate * Time.deltaTime;
        if (staminaRegenTimer <= 0) Stamina += StaminaRegenRate * Time.deltaTime;

        // ����ֵ�ķ�Χ
        Health = Mathf.Clamp(Health, 0, 100);
        Hunger = Mathf.Clamp(Hunger, 0, 100);
        Thirst = Mathf.Clamp(Thirst, 0, 100);
        Stamina = Mathf.Clamp(Stamina, 0, 100);

        // ��鼢���Ϳڿ��Ƿ�������ֵ�½�
        if (Hunger <= 0 || Thirst <= 0)
        {
            Health -= HealthDecayRate * Time.deltaTime;
        }

        // ����Ƿ�����
        if (Health <= 0)
        {
            Die();
        }
    }

    // �������״̬
    public void ResetState()
    {
        Health = 100;
        Hunger = 100;
        Thirst = 100;
        Stamina = 100;
        IsDead = false;
    }

    // ���ӻ�۳�����ֵ
    public void ModifyHealth(float amount)
    {
        Health += amount;
        healthRegenTimer = HealthRegenDelay;
    }

    // ���ӻ�۳�����ֵ
    public void ModifyHunger(float amount)
    {
        Hunger += amount;
        hungerDecayTimer = HungerDecayDelay;
    }

    // ���ӻ�۳��ڿ�ֵ
    public void ModifyThirst(float amount)
    {
        Thirst += amount;
        thirstDecayTimer = ThirstDecayDelay;
    }

    // ���ӻ�۳�����ֵ
    public void ModifyStamina(float amount)
    {
        Stamina += amount;
        staminaRegenTimer = StaminaRegenDelay;
    }

    // ֱ����������ֵ
    public void SetHealth(float value)
    {
        Health = value;
        healthRegenTimer = HealthRegenDelay;
    }

    // ֱ�����ü���ֵ
    public void SetHunger(float value)
    {
        Hunger = value;
        hungerDecayTimer = HungerDecayDelay;
    }

    // ֱ�����ÿڿ�ֵ
    public void SetThirst(float value)
    {
        Thirst = value;
        thirstDecayTimer = ThirstDecayDelay;
    }

    // ֱ����������ֵ
    public void SetStamina(float value)
    {
        Stamina = value;
        staminaRegenTimer = StaminaRegenDelay;
    }

    // �������
    private void Die()
    {
        IsDead = true;
    }

    // �������
    public void Respawn()
    {
        ResetState();
        if (PlayerNode != null)
        {
            PlayerNode.position = SpawnPos;
            PlayerNode.rotation = SpawnRot;
        }
    }

    // ���ó�����
    public void SetSpawnPoint(Transform Node)
    {
        PlayerNode = Node;
        SpawnPos = Node.position;
        SpawnRot = Node.rotation;
    }
}
