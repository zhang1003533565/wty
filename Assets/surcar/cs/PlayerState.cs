using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // 单例实例
    public static PlayerState Instance { get; private set; }

    // 玩家状态属性
    public float Health;
    public float Hunger;
    public float Thirst;
    public float Stamina;

    // 自然增长速度
    public float HealthRegenRate=0.2f;
    public float HungerDecayRate=-0.05f;
    public float ThirstDecayRate=-0.05f;
    public float StaminaRegenRate = 5f;

    // 外部改变后开始自然增长的延迟时间
    public float HealthRegenDelay = 5f;
    public float HungerDecayDelay = 30f;
    public float ThirstDecayDelay = 30f;
    public float StaminaRegenDelay = 0.5f;

    // 生命值扣除速度（当饥饿或口渴为0时）
    public float HealthDecayRate=0.5f;

    // 玩家死亡标志
    public bool IsDead;

    // 计时器
    private float healthRegenTimer;
    private float hungerDecayTimer;
    private float thirstDecayTimer;
    private float staminaRegenTimer;

    private Transform PlayerNode; // 玩家节点
    private Vector3 SpawnPos;
    private Quaternion SpawnRot;

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 初始化状态
        ResetState();
    }

    private void Update()
    {
        if (IsDead) return;

        // 更新计时器
        healthRegenTimer -= Time.deltaTime;
        hungerDecayTimer -= Time.deltaTime;
        thirstDecayTimer -= Time.deltaTime;
        staminaRegenTimer -= Time.deltaTime;

        // 自然增长或衰减
        if (healthRegenTimer <= 0) Health += HealthRegenRate * Time.deltaTime;
        if (hungerDecayTimer <= 0) Hunger += HungerDecayRate * Time.deltaTime;
        if (thirstDecayTimer <= 0) Thirst += ThirstDecayRate * Time.deltaTime;
        if (staminaRegenTimer <= 0) Stamina += StaminaRegenRate * Time.deltaTime;

        // 限制值的范围
        Health = Mathf.Clamp(Health, 0, 100);
        Hunger = Mathf.Clamp(Hunger, 0, 100);
        Thirst = Mathf.Clamp(Thirst, 0, 100);
        Stamina = Mathf.Clamp(Stamina, 0, 100);

        // 检查饥饿和口渴是否导致生命值下降
        if (Hunger <= 0 || Thirst <= 0)
        {
            Health -= HealthDecayRate * Time.deltaTime;
        }

        // 检查是否死亡
        if (Health <= 0)
        {
            Die();
        }
    }

    // 重置玩家状态
    public void ResetState()
    {
        Health = 100;
        Hunger = 100;
        Thirst = 100;
        Stamina = 100;
        IsDead = false;
    }

    // 增加或扣除生命值
    public void ModifyHealth(float amount)
    {
        Health += amount;
        healthRegenTimer = HealthRegenDelay;
    }

    // 增加或扣除饥饿值
    public void ModifyHunger(float amount)
    {
        Hunger += amount;
        hungerDecayTimer = HungerDecayDelay;
    }

    // 增加或扣除口渴值
    public void ModifyThirst(float amount)
    {
        Thirst += amount;
        thirstDecayTimer = ThirstDecayDelay;
    }

    // 增加或扣除耐力值
    public void ModifyStamina(float amount)
    {
        Stamina += amount;
        staminaRegenTimer = StaminaRegenDelay;
    }

    // 直接设置生命值
    public void SetHealth(float value)
    {
        Health = value;
        healthRegenTimer = HealthRegenDelay;
    }

    // 直接设置饥饿值
    public void SetHunger(float value)
    {
        Hunger = value;
        hungerDecayTimer = HungerDecayDelay;
    }

    // 直接设置口渴值
    public void SetThirst(float value)
    {
        Thirst = value;
        thirstDecayTimer = ThirstDecayDelay;
    }

    // 直接设置耐力值
    public void SetStamina(float value)
    {
        Stamina = value;
        staminaRegenTimer = StaminaRegenDelay;
    }

    // 玩家死亡
    private void Die()
    {
        IsDead = true;
    }

    // 玩家重生
    public void Respawn()
    {
        ResetState();
        if (PlayerNode != null)
        {
            PlayerNode.position = SpawnPos;
            PlayerNode.rotation = SpawnRot;
        }
    }

    // 设置出生点
    public void SetSpawnPoint(Transform Node)
    {
        PlayerNode = Node;
        SpawnPos = Node.position;
        SpawnRot = Node.rotation;
    }
}
