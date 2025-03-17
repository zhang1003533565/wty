using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private PlayerEquipmentManager equipmentManager;
    public Animator animator;
    private GameObject currentWeaponInstance; // 当前武器的实例
    private IWeapon currentWeapon; // 当前武器的逻辑接口
    private string currentWeaponId; // 当前武器的ID
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
        // 查找所有层级的子物体（包含非激活物体）
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
            { "axe", FindDeepChild("AxeMountPoint") }, // 斧头的挂载点
            { "fist", FindDeepChild("FistMountPoint") } // 拳头的挂载点
        };
    }

    private void Update()
    {
        // 检查武器是否变化
        string newWeaponId = equipmentManager.GetEquipment("Weapon");
        if (newWeaponId != currentWeaponId)
        {
            SyncWeapon();
        }

        // 如果处于空闲状态，播放武器的空闲动画
        /*if (currentWeapon != null)
        {
            PlayIdleAnimation();
        }*/

        //如果按下攻击按钮
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
            currentWeapon.OnAttack(); // 调用武器的攻击逻辑
        }
    }

    private void PlayIdleAnimation()
    {
        // 播放当前武器的空闲动画
        string idleAnimation = currentWeapon.GetIdleAnimationName();
        animator.Play(idleAnimation);
    }

    private void PlayAttackAnimation()
    {
        // 播放当前武器的攻击动画
        string attackAnimation = currentWeapon.GetAttackAnimationName();
        animator.Play(attackAnimation);
    }

    private void SyncWeapon()
    {
        string newWeaponId = equipmentManager.GetEquipment("Weapon");

        // 如果未装备武器，则使用默认的拳头武器
        if (string.IsNullOrEmpty(newWeaponId))
        {
            newWeaponId = "fist";
        }

        // 如果武器没有变化，直接返回
        if (newWeaponId == currentWeaponId)
        {
            return;
        }

        // 销毁当前武器实例
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
        }

        // 根据装备管理器加载新的武器预制体
        GameObject weaponPrefab = null;
        if (newWeaponId == "axe")
        {
            weaponPrefab = Resources.Load<GameObject>("weapon/axePrefab"); // 斧头的预制体路径
        }
        else if (newWeaponId == "fist")
        {
            weaponPrefab = Resources.Load<GameObject>("weapon/fistPrefab"); // 拳头的预制体路径
        }

        // 实例化武器并绑定到对应的挂载点
        if (weaponPrefab != null && weaponMountPoints.ContainsKey(newWeaponId))
        {
            Transform mountPoint = weaponMountPoints[newWeaponId];
            // 实例化武器并设置父节点
            currentWeaponInstance = Instantiate(weaponPrefab, mountPoint);
            // 将本地位置和旋转归零以确保对齐到父节点原点
            currentWeaponInstance.transform.localPosition = Vector3.zero;
            currentWeaponInstance.transform.localRotation = Quaternion.identity;

            currentWeapon = currentWeaponInstance.GetComponent<IWeapon>();
        }

        // 更新当前武器ID
        currentWeaponId = newWeaponId;

        //播放当前武器的空闲动画
        PlayIdleAnimation();
    }
}