using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public RectTransform fishTransform;
    public float controlSpeed = 50f; // 控制速度，可在Inspector中设置

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveDirection.y += controlSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveDirection.y -= controlSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveDirection.x -= controlSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveDirection.x += controlSpeed * Time.deltaTime;

        fishTransform.anchoredPosition += moveDirection;
    }
}