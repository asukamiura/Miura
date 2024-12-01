using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject savePrefab;
    GameObject saveObj;
    SaveManager saveManager;

    void Awake()
    {
        saveObj = Instantiate(savePrefab);
        saveObj.name = "SaveManager";       
        saveManager = saveObj.GetComponent<SaveManager>();
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        
    }
}
