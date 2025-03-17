public interface IWeapon
{
    string GetIdleAnimationName(); // 获取空闲动画名称
    string GetAttackAnimationName(); // 获取攻击动画名称
    void OnAttack(); // 攻击逻辑

    bool CanAttack();//返回武器是否现在可以进行攻击
}