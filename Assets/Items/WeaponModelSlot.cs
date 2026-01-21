using UnityEngine;

[RequireComponent(typeof(GearSlot))]
public class WeaponModelSlot : MonoBehaviour
{
    public GameObject currentWeapon;
    public GearSlot slot;

    [SerializeField] PlayerManager playerManager;

    public void UnloadWeapon()
    {
        if (currentWeapon != null)
        {
            playerManager.animations.Set2HSword(false);
            Destroy(currentWeapon);
        }
    }

    public void LoadWeapon(WeaponItem weapon)
    {
        UnloadWeapon();

        currentWeapon = Instantiate(weapon.gameObject, this.transform);

        if (weapon.isTwoHand == true)
        {
            playerManager.animations.Set2HSword(true);
        } else playerManager.animations.Set2HSword(false);
    }
}
