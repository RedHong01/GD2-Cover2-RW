using UnityEngine;

public class GoalChecker : MonoBehaviour
{
    public RectTransform fishTransform;
    public RectTransform targetArea;

    public bool IsFishInTarget()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(targetArea, fishTransform.anchoredPosition);
    }
}