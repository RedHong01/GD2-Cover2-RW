using UnityEngine;
using UnityEngine.UI;

public class QTECircle : MonoBehaviour
{
    // QTE圆圈，用于缩小的动画
    public Image qteCircle; // The shrinking QTE circle for the shrinking animation

    // 判定目标圆圈，用于成功判定
    public Image targetCircle; // The target circle for the success judgement

    // QTE圆圈的缩小速度，控制缩小的速度
    public float shrinkSpeed = 1f; // Shrinking speed of the QTE circle

    // 成功判定的距离范围，用于判断两圆圈是否重合
    public float successRange = 0.1f; // Success range for determining overlap

    // QTE成功标志，成功时设置为true，便于外部检查
    [HideInInspector] public bool isSuccessTriggered = false; // QTE success flag, set to true on success for external checking

    // QTE失败标志，失败时设置为true，便于外部检查
    [HideInInspector] public bool isFailureTriggered = false; // QTE failure flag, set to true on failure for external checking

    private Vector3 originalSize; // QTE圆圈的原始大小，用于重置
    private bool isActive = false; // QTE是否处于激活状态

    void Start()
    {
        originalSize = qteCircle.rectTransform.localScale; // 保存QTE圆圈的初始大小
        // Save the initial size of the QTE circle for resetting later
    }

    void Update()
    {
        if (isActive)
        {
            // 如果QTE激活，逐渐缩小QTE圆圈
            // If QTE is active, shrink the QTE circle
            qteCircle.rectTransform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

            // 计算QTE圆圈与目标圆圈之间的距离
            // Calculate the distance between QTE and target circle
            float distance = Vector3.Distance(qteCircle.rectTransform.localScale, targetCircle.rectTransform.localScale);

            // 如果距离在成功范围内且按下空格键，则判定为成功
            // If distance is within success range and space key is pressed, it's a success
            if (distance <= successRange && !isSuccessTriggered && Input.GetKeyDown(KeyCode.Space))
            {
                isSuccessTriggered = true; // 设置成功标志
                isActive = false; // 停止QTE
                OnQTESuccess(); // 调用成功逻辑
            }

            // 如果QTE圆圈小于目标圆圈且未成功，则判定为失败
            // If QTE circle is smaller than target circle and not successful, it's a failure
            if (qteCircle.rectTransform.localScale.x < targetCircle.rectTransform.localScale.x && !isSuccessTriggered)
            {
                isFailureTriggered = true; // 设置失败标志
                isActive = false; // 停止QTE
                OnQTEFail(); // 调用失败逻辑
            }
        }
    }

    // 启动QTE并重置状态
    // Start the QTE and reset its status
    public void StartQTE()
    {
        isActive = true; // 激活QTE
        isSuccessTriggered = false; // 重置成功标志
        isFailureTriggered = false; // 重置失败标志
        qteCircle.rectTransform.localScale = originalSize; // 重置QTE圆圈大小
    }

    // 成功逻辑，打印成功信息
    // Success logic, prints success message
    public void OnQTESuccess()
    {
        Debug.Log("QTE 成功!"); // 输出成功信息
        // Additional success effects or animations can be added here
    }

    // 失败逻辑，打印失败信息
    // Failure logic, prints failure message
    public void OnQTEFail()
    {
        Debug.Log("QTE 失败!"); // 输出失败信息
        // Additional failure effects or restart QTE can be added here
    }
}