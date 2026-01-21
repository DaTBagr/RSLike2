using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerStats stats;
    public PlayerInfo info;
    public PlayerEquipment equipment;
    public PlayerAnimations animations;

    public GearItem hSword;

    private void Start()
    {
        animations = GetComponent<PlayerAnimations>();
    }
}
