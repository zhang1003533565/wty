using UnityEngine;
using System.Collections;

public class EnemyAIBasic : MonoBehaviour
{
    public float moveSpeed = 3.0f; // �����ƶ��ٶ� 
    public float stopAfterDamageTime = 2.0f; // �ܵ�������ֹͣ�ƶ���ʱ��
    public float detectionRange = 10.0f; // �����ҵķ�Χ
    public float trackingRange = 20.0f; // ׷����ҵķ�Χ
    public LayerMask playerLayer; // ��ҵ�Layer
    public float stopDistance = 1.5f; // ֹͣ�ƶ��ľ���
    public float attackRange = 2.0f; // ������Χ
    public float attackCooldown = 1.0f; // ������ȴʱ��
    public float attackAfterCooldown = 1.0f; // ������ȴʱ��
    public int attackDamage = 10; // �����˺�
    public string weaponName = "EnemyFist"; // ��������

    private Transform player; // ��ǰ׷�ٵ����
    private Rigidbody rb; // �����Rigidbody���
    public Animator animator; // ����������
    private bool isTracking = false; // �Ƿ�����׷�����
    private bool isStopped = false; // �Ƿ�ֹͣ�ƶ�
    private bool isAttacking = false; // �Ƿ����ڹ���
    private Vector3 lastKnownPlayerPosition; // ��ҵ������֪λ��

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip trackingSound; // ׷��ʱ���ŵ���Ч
    public AudioClip attackSound; // ����ʱ���ŵ���Ч

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>(); // ��ȡ�������������
        // ��ʼ��ʱ�������
        SearchForPlayer();
    }

    void Update()
    {
        // �������������ֹͣ�ƶ�����ֱ�ӷ���
        if (isStopped || GetComponent<IInteractable>().isDeath()) return;

        // ���û����׷����ң����������
        if (!isTracking)
        {
            SearchForPlayer();
        }
        else
        {
            TrackPlayer();
        }
    }

    // �������
    void SearchForPlayer()
    {
        // ʹ��OverlapSphere��ⷶΧ�ڵ����
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestPlayer = null;

        // �������м�⵽����ң��ҵ���������
        foreach (var hitCollider in hitColliders)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = hitCollider.transform;
            }
        }

        // ����ҵ���ң���ʼ׷��
        if (nearestPlayer != null)
        {
            player = nearestPlayer;
            isTracking = true;

            //������Ч
            audioSource.PlayOneShot(trackingSound);
        }
    }

    // ׷�����
    void TrackPlayer()
    {
        if (player == null)
            return;

        // �����ҳ���׷�ٷ�Χ����ֹͣ׷��
        if (Vector3.Distance(transform.position, player.position) > trackingRange)
        {
            isTracking = false;
            animator.SetBool("walk", false); // ֹͣ�������߶���
            return;
        }

        // ���㳯����ҵ�ˮƽ���򣨺���Y�ᣩ
        Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;

        // ���������Ҵ���ֹͣ���룬������ƶ�
        if (Vector3.Distance(transform.position, player.position) > stopDistance)
        {
            MoveTowards(direction);
            animator.SetBool("walk", true); // �������߶���
        }
        else
        {
            animator.SetBool("walk", false); // ֹͣ�������߶���
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackPlayer());
            }
        }

        // �����ƶ��ķ��򣨽���Y������ת��
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }

    // �ƶ�����
    void MoveTowards(Vector3 direction)
    {
        // ʹ�����߼���ϰ���
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, moveSpeed * Time.deltaTime))
        {
            // �����⵽�Ĳ�����ң����ƹ��ϰ���
            if (hit.collider.gameObject.layer != playerLayer)
            {
                direction = AvoidObstacle(direction, hit.normal);
            }
        }

        // ʹ��Rigidbody�ƶ�������⴩ǽ
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    // �ƹ��ϰ���
    Vector3 AvoidObstacle(Vector3 direction, Vector3 normal)
    {
        // ʹ�÷��������ƹ��ϰ���
        return Vector3.Reflect(direction, normal);
    }

    // �������ܵ�����ʱ���߼�
    void OnTakeDamage()
    {
        StartCoroutine(StopMovement());
    }

    // ֹͣ�ƶ���Э��
    IEnumerator StopMovement()
    {
        isStopped = true;
        animator.SetBool("walk", false); // ֹͣ�������߶���
        yield return new WaitForSeconds(stopAfterDamageTime);
        isStopped = false;
    }

    // ������ҵ�Э��
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("attack"); // ������������
        audioSource.PlayOneShot(attackSound); // ���Ź�����Ч

        //Debug.Log("Attack Start.");

        yield return new WaitForSeconds(attackCooldown); // �ȴ����������������

        // �ж�����Ƿ����ڹ�����Χ��
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // ��ȡ��Ҷ����IInteractable�ӿ�
            IInteractable playerInteractable = player.GetComponent<IInteractable>();
            if (playerInteractable != null)
            {
                // ���������˺�
                playerInteractable.TakeDamage(weaponName, attackDamage);
            }
        }

        //Debug.Log("Attack inter");

        yield return new WaitForSeconds(attackAfterCooldown); // ������ɺ�ԭ�ط���ʱ��

        //Debug.Log("Attack finish");

        isAttacking = false;
    }

    // ���Ƽ�ⷶΧ��׷�ٷ�Χ��Gizmos�������ã�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);
    }
}
