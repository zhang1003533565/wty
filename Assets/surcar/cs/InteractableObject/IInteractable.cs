public interface IInteractable
{
    bool IsInteractable();
    bool IsAttackable();
    string GetName();
    void Interact();
    void TakeDamage(string weaponName, int damage);

    bool isDeath();
}
