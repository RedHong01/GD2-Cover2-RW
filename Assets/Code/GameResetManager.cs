using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 使用SceneManager的命名空间 // Namespace for SceneManager

public class GameResetManager : MonoBehaviour
{
    // 重置游戏的按钮 // Button to reset the game
    public Button resetButton;

    void Start()
    {
        // 订阅按钮的点击事件 // Subscribe the button click event
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(ResetGame);
        }
    }

    void Update()
    {
        // 检查是否按下了R键 // Check if the R key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    void ResetGame()
    {
        // 重新加载当前场景 // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Game reset to initial state by reloading the scene.");
    }
}