using UnityEngine;
using System.Collections;

public class EnemyAIBasic : MonoBehaviour
{
    public float moveSpeed = 3.0f; // 怪物移动速度 
    public float stopAfterDamageTime = 2.0f; // 受到攻击后停止移动的时间
    public float detectionRange = 10.0f; // 检测玩家的范围
    public float trackingRange = 20.0f; // 追踪玩家的范围
    public LayerMask playerLayer; // 玩家的Layer
    public float stopDistance = 1.5f; // 停止移动的距离
    public float attackRange = 2.0f; // 攻击范围
    public float attackCooldown = 1.0f; // 攻击冷却时间
    public float attackAfterCooldown = 1.0f; // 攻击冷却时间
    public int attackDamage = 10; // 攻击伤害
    public string weaponName = "EnemyFist"; // 武器名称

    private Transform player; // 当前追踪的玩家
    private Rigidbody rb; // 怪物的Rigidbody组件
    public Animator animator; // 动画控制器
    private bool isTracking = false; // 是否正在追踪玩家
    private bool isStopped = false; // 是否停止移动
    private bool isAttacking = false; // 是否正在攻击
    private Vector3 lastKnownPlayerPosition; // 玩家的最后已知位置

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip trackingSound; // 追踪时播放的音效
    public AudioClip attackSound; // 攻击时播放的音效

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>(); // 获取动画控制器组件
        // 初始化时搜索玩家
        SearchForPlayer();
    }

    void Update()
    {
        // 如果怪物死亡或停止移动，则直接返回
        if (isStopped || GetComponent<IInteractable>().isDeath()) return;

        // 如果没有在追踪玩家，则搜索玩家
        if (!isTracking)
        {
            SearchForPlayer();
        }
        else
        {
            TrackPlayer();
        }
    }

    // 搜索玩家
    void SearchForPlayer()
    {
        // 使用OverlapSphere检测范围内的玩家
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestPlayer = null;

        // 遍历所有检测到的玩家，找到最近的玩家
        foreach (var hitCollider in hitColliders)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = hitCollider.transform;
            }
        }

        // 如果找到玩家，则开始追踪
        if (nearestPlayer != null)
        {
            player = nearestPlayer;
            isTracking = true;

            //播放音效
            audioSource.PlayOneShot(trackingSound);
        }
    }

    // 追踪玩家
    void TrackPlayer()
    {
        if (player == null)
            return;

        // 如果玩家超出追踪范围，则停止追踪
        if (Vector3.Distance(transform.position, player.position) > trackingRange)
        {
            isTracking = false;
            animator.SetBool("walk", false); // 停止播放行走动画
            return;
        }

        // 计算朝向玩家的水平方向（忽略Y轴）
        Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;

        // 如果距离玩家大于停止距离，则继续移动
        if (Vector3.Distance(transform.position, player.position) > stopDistance)
        {
            MoveTowards(direction);
            animator.SetBool("walk", true); // 播放行走动画
        }
        else
        {
            animator.SetBool("walk", false); // 停止播放行走动画
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackPlayer());
            }
        }

        // 朝向移动的方向（仅在Y轴上旋转）
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }

    // 移动怪物
    void MoveTowards(Vector3 direction)
    {
        // 使用射线检测障碍物
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, moveSpeed * Time.deltaTime))
        {
            // 如果检测到的不是玩家，则绕过障碍物
            if (hit.collider.gameObject.layer != playerLayer)
            {
                direction = AvoidObstacle(direction, hit.normal);
            }
        }

        // 使用Rigidbody移动怪物，避免穿墙
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    // 绕过障碍物
    Vector3 AvoidObstacle(Vector3 direction, Vector3 normal)
    {
        // 使用反射向量绕过障碍物
        return Vector3.Reflect(direction, normal);
    }

    // 当怪物受到攻击时的逻辑
    void OnTakeDamage()
    {
        StartCoroutine(StopMovement());
    }

    // 停止移动的协程
    IEnumerator StopMovement()
    {
        isStopped = true;
        animator.SetBool("walk", false); // 停止播放行走动画
        yield return new WaitForSeconds(stopAfterDamageTime);
        isStopped = false;
    }

    // 攻击玩家的协程
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("attack"); // 触发攻击动画
        audioSource.PlayOneShot(attackSound); // 播放攻击音效

        //Debug.Log("Attack Start.");

        yield return new WaitForSeconds(attackCooldown); // 等待攻击动画播放完成

        // 判断玩家是否仍在攻击范围内
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // 获取玩家对象的IInteractable接口
            IInteractable playerInteractable = player.GetComponent<IInteractable>();
            if (playerInteractable != null)
            {
                // 对玩家造成伤害
                playerInteractable.TakeDamage(weaponName, attackDamage);
            }
        }

        //Debug.Log("Attack inter");

        yield return new WaitForSeconds(attackAfterCooldown); // 攻击完成后原地发呆时间

        //Debug.Log("Attack finish");

        isAttacking = false;
    }

    // 绘制检测范围和追踪范围的Gizmos（调试用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }
}
