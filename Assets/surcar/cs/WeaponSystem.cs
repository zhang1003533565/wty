using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private PlayerEquipmentManager equipmentManager;
    public Animator animator;
    private GameObject currentWeaponInstance; // ��ǰ������ʵ��
    private IWeapon currentWeapon; // ��ǰ�������߼��ӿ�
    private string currentWeaponId; // ��ǰ������ID
    private Dictionary<string, Transform> weaponMountPoints;

    private InputManager inputManager;

    private void Awake()
    {
        equipmentManager = PlayerEquipmentManager.Instance;
        inputManager = GetComponent<InputManager>();
        //animator = GetComponent<Animator>();
        InitializeMountPoints();
    }
    private Transform FindDeepChild(string name)
    {
        // �������в㼶�������壨�����Ǽ������壩
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == name)
                return child;
        }
        return null;
    }

    private void InitializeMountPoints()
    {
        weaponMountPoints = new Dictionary<string, Transform>
        {
            { "axe", FindDeepChild("AxeMountPoint") }, // ��ͷ�Ĺ��ص�
            { "fist", FindDeepChild("FistMountPoint") } // ȭͷ�Ĺ��ص�
        };
    }

    private void Update()
    {
        // ��������Ƿ�仯
        string newWeaponId = equipmentManager.GetEquipment("Weapon");
        if (newWeaponId != currentWeaponId)
        {
            SyncWeapon();
        }

        // ������ڿ���״̬�����������Ŀ��ж���
        /*if (currentWeapon != null)
        {
            PlayIdleAnimation();
        }*/

        //������¹�����ť
        if (inputManager.AttackPressed)
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (currentWeapon != null && currentWeapon.CanAttack())
        {
            PlayAttackAnimation();
            currentWeapon.OnAttack(); // ���������Ĺ����߼�
        }
    }

    private void PlayIdleAnimation()
    {
        // ���ŵ�ǰ�����Ŀ��ж���
        string idleAnimation = currentWeapon.GetIdleAnimationName();
        animator.Play(idleAnimation);
    }

    private void PlayAttackAnimation()
    {
        // ���ŵ�ǰ�����Ĺ�������
        string attackAnimation = currentWeapon.GetAttackAnimationName();
        animator.Play(attackAnimation);
    }

    private void SyncWeapon()
    {
        string newWeaponId = equipmentManager.GetEquipment("Weapon");

        // ���δװ����������ʹ��Ĭ�ϵ�ȭͷ����
        if (string.IsNullOrEmpty(newWeaponId))
        {
            newWeaponId = "fist";
        }

        // �������û�б仯��ֱ�ӷ���
        if (newWeaponId == currentWeaponId)
        {
            return;
        }

        // ���ٵ�ǰ����ʵ��
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
        }

        // ����װ�������������µ�����Ԥ����
        GameObject weaponPrefab = null;
        if (newWeaponId == "axe")
        {
            weaponPrefab = Resources.Load<GameObject>("weapon/axePrefab"); // ��ͷ��Ԥ����·��
        }
        else if (newWeaponId == "fist")
        {
            weaponPrefab = Resources.Load<GameObject>("weapon/fistPrefab"); // ȭͷ��Ԥ����·��
        }

        // ʵ�����������󶨵���Ӧ�Ĺ��ص�
        if (weaponPrefab != null && weaponMountPoints.ContainsKey(newWeaponId))
        {
            Transform mountPoint = weaponMountPoints[newWeaponId];
            // ʵ�������������ø��ڵ�
            currentWeaponInstance = Instantiate(weaponPrefab, mountPoint);
            // ������λ�ú���ת������ȷ�����뵽���ڵ�ԭ��
            currentWeaponInstance.transform.localPosition = Vector3.zero;
            currentWeaponInstance.transform.localRotation = Quaternion.identity;

            currentWeapon = currentWeaponInstance.GetComponent<IWeapon>();
        }

        // ���µ�ǰ����ID
        currentWeaponId = newWeaponId;

        //���ŵ�ǰ�����Ŀ��ж���
        PlayIdleAnimation();
    }
}