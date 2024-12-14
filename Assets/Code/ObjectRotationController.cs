using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    // 要旋转的GameObject // The GameObject to rotate
    public GameObject targetObject;

    // 旋转速度，可在Inspector中设置 // Rotation speed, configurable in the Inspector
    public float rotationSpeed = 100f;

    // Y轴旋转的最小值和最大值 // Minimum and maximum values for y-axis rotation
    public float minYRotation = -45f;
    public float maxYRotation = 45f;

    void Update()
    {
        // 检查是否按下了A键或左方向键 // Check if the A key or left arrow key is pressed
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RotateObject(-rotationSpeed);
        }

        // 检查是否按下了D键或右方向键 // Check if the D key or right arrow key is pressed
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RotateObject(rotationSpeed);
        }
    }

    // 旋转对象的方法 // Method to rotate the object
    private void RotateObject(float rotationAmount)
    {
        if (targetObject != null) // 检查目标对象是否不为空 // Check if the target object is not null
        {
            // 当前的Y轴旋转角度 // Current y-axis rotation angle
            float currentYRotation = targetObject.transform.eulerAngles.y;

            // 将Y轴旋转值转换为-180到180范围内以便正确限制 // Convert y-rotation to a range of -180 to 180 for proper clamping
            if (currentYRotation > 180)
            {
                currentYRotation -= 360;
            }

            // 计算新的Y轴旋转角度 // Calculate the new y-axis rotation angle
            float newYRotation = Mathf.Clamp(currentYRotation + rotationAmount * Time.deltaTime, minYRotation, maxYRotation);

            // 应用新的旋转角度 // Apply the new rotation angle
            targetObject.transform.eulerAngles = new Vector3(
                targetObject.transform.eulerAngles.x,
                newYRotation,
                targetObject.transform.eulerAngles.z
            );
        }
        else
        {
            Debug.LogWarning("Target object is not assigned."); // 如果目标对象未指定，输出警告 // Log a warning if the target object is not assigned
        }
    }
}