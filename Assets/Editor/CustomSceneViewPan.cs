using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class CustomSceneViewPan
{
    static Vector3 previousMousePosition;

    static CustomSceneViewPan()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // 检查是否按下 Shift + 左键
        if (e.shift && e.button == 0 && e.type == EventType.MouseDrag)
        {
            // 计算鼠标移动的差异
            Vector3 delta = e.delta;

            // 将移动量应用到 Scene 视图的相机位置
            sceneView.camera.transform.Translate(-delta.x * 0.01f, delta.y * 0.01f, 0);

            // 强制刷新视图
            sceneView.Repaint();

            // 阻止事件继续传递，表示已处理
            e.Use();
        }
    }
}