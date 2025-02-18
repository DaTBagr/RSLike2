using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] CharacterInfo player;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple GameManager objects");
            Destroy(gameObject);
        }
        instance = this;
    }

    public Unit GetPlayer()
    {
        return player;
    }
}
