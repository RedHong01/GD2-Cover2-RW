using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishEscape : MonoBehaviour
{
    // 基本的移动属性和组件引用
    // Basic movement attributes and component references
    public float baseMoveSpeed = 50f; // 基础移动速度 // Base movement speed
    public float speedVariation = 10f; // 速度的变化范围 // Speed variation range
    public float directionChangeInterval = 1f; // 方向更改的时间间隔 // Time interval for changing direction
    public RectTransform fishTransform; // 鱼的UI变换 // Fish's UI transform
    public RectTransform targetAreaTransform; // 目标区域的UI变换 // Target area's UI transform
    public RectTransform canvasRect; // UI画布，用于判定鱼的位置 // UI Canvas for fish position boundary check
    public TextMeshProUGUI countdownText; // 倒计时文本 // Countdown text
    public TextMeshProUGUI defeatText; // 失败文本 // Defeat text
    public float escapeDuration = 5f; // 逃脱的时间 // Escape duration

    public QTECircle qteCircle; // 引用QTECircle脚本，用于触发QTE // Reference to QTECircle script to trigger QTE

    private Vector2 originalPosition; // 鱼的初始位置 // Initial position of the fish
    private Vector2 currentDirection; // 当前方向 // Current direction
    private float currentSpeed; // 当前速度 // Current speed
    private float directionChangeTimer; // 方向更改计时器 // Timer for direction change

    public bool fishingSucceeded = false; // 钓鱼成功标志 // Fishing success flag
    public bool fishingFailed = false; // 钓鱼失败标志 // Fishing failure flag

    void Start()
    {
        originalPosition = fishTransform.anchoredPosition; // 保存鱼的初始位置 // Save initial position of the fish
        currentSpeed = baseMoveSpeed; // 设置初始速度 // Set initial speed
        ChangeDirection(); // 随机更改鱼的初始方向 // Change fish's initial direction randomly
        StartCoroutine(EscapeRoutine()); // 启动逃脱协程 // Start escape coroutine
        HideResultTexts(); // 隐藏结果文本 // Hide result texts
    }

    // 控制鱼的逃脱逻辑
    // Controls fish escape logic
    IEnumerator EscapeRoutine()
    {
        float timer = escapeDuration; // 逃脱计时器 // Escape timer
        directionChangeTimer = directionChangeInterval; // 初始化方向更改计时器 // Initialize direction change timer

        while (timer > 0)
        {
            countdownText.text = "Time Left: " + Mathf.Ceil(timer).ToString() + "s"; // 更新倒计时文本 // Update countdown text
            timer -= Time.deltaTime; // 减少计时器 // Reduce timer
            directionChangeTimer -= Time.deltaTime; // 减少方向计时器 // Reduce direction timer

            // 检查是否需要更改方向
            // Check if it's time to change direction
            if (directionChangeTimer <= 0)
            {
                ChangeDirection(); // 更改鱼的移动方向 // Change fish movement direction
                directionChangeTimer = directionChangeInterval; // 重置方向计时器 // Reset direction timer
            }

            // 计算新位置
            // Calculate new position
            Vector2 newPosition = fishTransform.anchoredPosition + currentDirection * currentSpeed * Time.deltaTime;
            if (IsWithinCanvasBounds(newPosition)) // 检查是否在画布范围内 // Check if within canvas bounds
            {
                fishTransform.anchoredPosition = newPosition; // 更新鱼的位置 // Update fish position
            }

            yield return null; // 等待下一帧 // Wait for next frame
        }

        countdownText.text = ""; // 清空倒计时文本 // Clear countdown text
        CheckVictoryCondition(); // 检查胜利条件 // Check victory condition
    }

    // 更改鱼的随机移动方向
    // Change fish's random movement direction
    void ChangeDirection()
    {
        currentDirection = Random.insideUnitCircle.normalized; // 随机生成方向 // Generate random direction
        currentSpeed = baseMoveSpeed + Random.Range(-speedVariation, speedVariation); // 随机生成速度 // Generate random speed
    }

    // 判断新位置是否在画布范围内
    // Check if the new position is within canvas bounds
    bool IsWithinCanvasBounds(Vector2 position)
    {
        Rect canvasRectBounds = new Rect(
            -canvasRect.rect.width / 2, -canvasRect.rect.height / 2,
            canvasRect.rect.width, canvasRect.rect.height
        );
        return canvasRectBounds.Contains(position); // 检查位置是否在范围内 // Check if position is within bounds
    }

    // 检查鱼是否在目标区域内
    // Check if the fish is within the target area
    void CheckVictoryCondition()
    {
        Rect fishBounds = GetRectFromRectTransform(fishTransform); // 获取鱼的边界 // Get fish bounds
        Rect targetBounds = GetRectFromRectTransform(targetAreaTransform); // 获取目标区域的边界 // Get target area bounds

        if (targetBounds.Overlaps(fishBounds)) // 如果鱼在目标区域内 // If fish is within target area
        {
            StartCoroutine(DisplayResult(true)); // 启动QTE // Start QTE
        }
        else // 如果鱼不在目标区域内 // If fish is not within target area
        {
            StartCoroutine(DisplayResult(false)); // 显示失败信息 // Show defeat message
        }
    }

    // 显示结果的协程，控制QTE逻辑
    // Coroutine to display result and control QTE logic
    IEnumerator DisplayResult(bool isSuccess)
    {
        if (isSuccess)
        {
            qteCircle.StartQTE(); // 启动QTE // Start QTE

            // 等待QTE结果
            // Wait for QTE result
            yield return new WaitUntil(() => qteCircle.isSuccessTriggered || qteCircle.isFailureTriggered);

            if (qteCircle.isSuccessTriggered) // QTE成功
            {
                fishingSucceeded = true; // 设置钓鱼成功标志 // Set fishing success flag
                Debug.Log("钓鱼成功!"); // 输出成功信息 // Output success message
            }
            else if (qteCircle.isFailureTriggered) // QTE失败
            {
                fishingFailed = true; // 设置钓鱼失败标志 // Set fishing failure flag
                defeatText.gameObject.SetActive(true); // 显示失败文本 // Show defeat text
                defeatText.text = "Defeat! The fish got away!";
            }
        }
        else // 如果鱼不在目标区域
        {
            defeatText.gameObject.SetActive(true); // 显示失败文本 // Show defeat text
            defeatText.text = "Defeat! The fish got away!";
            fishingFailed = true; // 设置钓鱼失败标志 // Set fishing failure flag
        }

        yield return new WaitForSeconds(3f); // 等待3秒 // Wait for 3 seconds
    }

    // 获取RectTransform的矩形范围
    // Get rectangle bounds from RectTransform
    Rect GetRectFromRectTransform(RectTransform rectTransform)
    {
        Vector2 size = rectTransform.rect.size * rectTransform.lossyScale; // 计算大小 // Calculate size
        Vector2 position = rectTransform.anchoredPosition; // 获取位置 // Get position
        return new Rect(position.x - size.x / 2, position.y - size.y / 2, size.x, size.y); // 返回范围 // Return bounds
    }

    // 隐藏结果文本
    // Hide result texts
    void HideResultTexts()
    {
        defeatText.gameObject.SetActive(false); // 隐藏失败文本 // Hide defeat text
    }
}