public interface IWeapon
{
    string GetIdleAnimationName(); // ��ȡ���ж�������
    string GetAttackAnimationName(); // ��ȡ������������
    void OnAttack(); // �����߼�

    bool CanAttack();//���������Ƿ����ڿ��Խ��й���
}