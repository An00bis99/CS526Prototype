using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    private Menu menuController;

    private void Awake()
    {
        menuController = new Menu();
    }

    private void OnEnable()
    {
        menuController.Enable();
    }

    private void OnDisable()
    {
        menuController.Disable();
    }

    void Update()
    {
        if (menuController.PlayerMenu.Confirm.IsPressed())
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}