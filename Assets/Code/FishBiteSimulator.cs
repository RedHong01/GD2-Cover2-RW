using UnityEngine;
using System.Collections;

public class FishBiteSimulator : MonoBehaviour
{
    public GameObject buoy;

    // 慢速漂浮的参数 // Parameters for Slow Float
    public float slowFloatSpeed = 0.5f; // 慢速漂浮的速度 // Speed for slow float
    public float slowFloatMinY = 0f; // 慢速漂浮的最小Y位置 // Minimum Y position for slow float
    public float slowFloatMaxY = 1f; // 慢速漂浮的最大Y位置 // Maximum Y position for slow float

    // 快速漂浮的参数 // Parameters for Rapid Float
    public float rapidFloatSpeed = 2f; // 快速漂浮的速度 // Speed for rapid float
    public float rapidFloatMinY = 0.2f; // 快速漂浮的最小Y位置 // Minimum Y position for rapid float
    public float rapidFloatMaxY = 1.5f; // 快速漂浮的最大Y位置 // Maximum Y position for rapid float
    public float rapidFloatDuration = 5f; // 快速漂浮的持续时间（上钩时间） // Duration for rapid float (hooked duration)

    // 虚假漂浮的参数 // Parameters for Fake Float
    public float fakeFloatSpeed = 1f; // 虚假漂浮的速度 // Speed for fake float
    public float fakeFloatMinY = -0.5f; // 虚假漂浮的最小Y位置 // Minimum Y position for fake float
    public float fakeFloatMaxY = 0.5f; // 虚假漂浮的最大Y位置 // Maximum Y position for fake float
    public float fakeFloatDuration = 3f; // 虚假漂浮的持续时间 // Duration for fake float

    public float stateChangeInterval = 10f; // 慢速漂浮的切换间隔 // Interval for slow float state duration

    public GameManager gameManager; // 游戏管理器引用 // Reference to the GameManager

    // 状态触发的权重参数 // Probability weights for each state
    public float slowFloatWeight = 1f; // 慢速漂浮的权重 // Weight for slow float
    public float rapidFloatWeight = 1f; // 快速漂浮的权重 // Weight for rapid float
    public float fakeFloatWeight = 1f; // 虚假漂浮的权重 // Weight for fake float

    public GameObject hookedIndicator; // 在上钩状态下显示的对象 // GameObject to activate when hooked

    private bool isFloatingUp = true; // 控制漂浮方向的布尔值 // Boolean for floating direction control
    private float floatSpeed; // 当前漂浮速度 // Current floating speed
    private float minY; // 当前状态的最小Y值 // Minimum Y for the current state
    private float maxY; // 当前状态的最大Y值 // Maximum Y for the current state
    private bool isHooked; // 记录上一次的hooked状态 // Record the previous hooked state

    void Start()
    {
        SetFloatingState("Slow"); // 初始状态设置为慢速漂浮 // Initialize with Slow Float state
        StartCoroutine(SwitchFloatingStates()); // 开始状态切换协程 // Start coroutine for switching states
        UpdateHookedIndicator(); // 初始化hooked指示器状态 // Initialize the hooked indicator state
    }

    void Update()
    {
        FloatBuoy(); // 每帧执行浮标漂浮逻辑 // Call FloatBuoy every frame
    }

    private void FloatBuoy()
    {
        Vector3 position = buoy.transform.position; // 获取浮标当前位置 // Get the current position of the buoy

        // 根据方向调整位置 // Adjust position based on direction
        if (isFloatingUp)
        {
            position.y += floatSpeed * Time.deltaTime; // 向上移动 // Move up
            if (position.y >= maxY) // 达到最大Y值后改变方向 // Change direction when reaching maxY
            {
                position.y = maxY;
                isFloatingUp = false;
            }
        }
        else
        {
            position.y -= floatSpeed * Time.deltaTime; // 向下移动 // Move down
            if (position.y <= minY) // 达到最小Y值后改变方向 // Change direction when reaching minY
            {
                position.y = minY;
                isFloatingUp = true;
            }
        }

        buoy.transform.position = position; // 更新浮标位置 // Update the buoy's position
    }

    private void SetFloatingState(string state)
    {
        switch (state)
        {
            case "Slow":
                floatSpeed = slowFloatSpeed; // 设置慢速漂浮速度 // Set speed for slow float
                minY = slowFloatMinY; // 设置慢速漂浮的最小Y值 // Set min Y for slow float
                maxY = slowFloatMaxY; // 设置慢速漂浮的最大Y值 // Set max Y for slow float
                UpdateHookedState(false); // 慢速漂浮时未上钩 // Not hooked in slow float
                break;

            case "Rapid":
                floatSpeed = rapidFloatSpeed; // 设置快速漂浮速度 // Set speed for rapid float
                minY = rapidFloatMinY; // 设置快速漂浮的最小Y值 // Set min Y for rapid float
                maxY = rapidFloatMaxY; // 设置快速漂浮的最大Y值 // Set max Y for rapid float
                UpdateHookedState(true); // 快速漂浮时为上钩状态 // Hooked in rapid float
                break;

            case "Fake":
                floatSpeed = fakeFloatSpeed; // 设置虚假漂浮速度 // Set speed for fake float
                isFloatingUp = false; // 使浮标向下移动 // Set direction to move downwards
                minY = fakeFloatMinY; // 设置虚假漂浮的最小Y值 // Set min Y for fake float
                maxY = fakeFloatMaxY; // 设置虚假漂浮的最大Y值 // Set max Y for fake float
                UpdateHookedState(false); // 虚假漂浮时未上钩 // Not hooked in fake float
                break;
        }
    }

    // 更新上钩状态并控制hooked指示器 // Update hooked state and control hooked indicator
    private void UpdateHookedState(bool hooked)
    {
        if (gameManager != null)
        {
            gameManager.isFishHooked = hooked; // 更新GameManager的isFishHooked状态 // Update isFishHooked in GameManager
        }
        isHooked = hooked; // 更新本地hooked状态 // Update local hooked state
        UpdateHookedIndicator(); // 更新指示器状态 // Update the indicator status
    }

    // 根据上钩状态设置指示器的active状态 // Set indicator active state based on hooked status
    private void UpdateHookedIndicator()
    {
        if (hookedIndicator != null)
        {
            hookedIndicator.SetActive(isHooked); // 设置指示器是否激活 // Set indicator active or inactive
        }
    }

    private string GetRandomState()
    {
        // 计算总权重 // Calculate the total weight
        float totalWeight = slowFloatWeight + rapidFloatWeight + fakeFloatWeight;
        float randomValue = Random.Range(0, totalWeight); // 生成随机值 // Generate random value

        // 根据随机值选择状态 // Select the state based on random value
        if (randomValue < slowFloatWeight)
        {
            return "Slow";
        }
        else if (randomValue < slowFloatWeight + rapidFloatWeight)
        {
            return "Rapid";
        }
        else
        {
            return "Fake";
        }
    }

    private IEnumerator SwitchFloatingStates()
    {
        while (true)
        {
            string nextState = GetRandomState(); // 获取下一个随机状态 // Get the next random state
            SetFloatingState(nextState); // 设置新的状态 // Set the new state

            float duration = nextState == "Rapid" ? rapidFloatDuration : nextState == "Fake" ? fakeFloatDuration : stateChangeInterval; // 设置状态持续时间 // Set duration based on selected state
            yield return new WaitForSeconds(duration); // 等待相应的持续时间 // Wait for the specified duration
        }
    }
}