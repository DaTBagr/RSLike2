using UnityEngine;

public class WeaponModelSlot : MonoBehaviour
{
    public GameObject currentWeapon;

    public void UnloadWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
    }

    public void LoadWeapon(GameObject weapon)
    {
        if (currentWeapon != null)
        {
            UnloadWeapon();
        }

        currentWeapon = weapon;

        weapon.transform.parent = transform;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localScale = Vector3.one;
    }
}
