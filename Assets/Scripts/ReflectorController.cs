using UnityEngine;
using UnityEngine.InputSystem;

public class ReflectorController : MonoBehaviour
{
    [SerializeField] float rotationAnglePerClick = 45f; // 1クリックあたりの回転角度
    [SerializeField] LayerMask mirrorLayer;      // Reflector のレイヤー
    [SerializeField] float minAngle = -90f;      // 最小回転角度
    [SerializeField] float maxAngle = 90f;       // 最大回転角度

    Camera mainCam;
    Quaternion startRotation;
    float currentRotationY = 0f; // 現在のY軸回転角度
    int rotationDirection = 1; // 回転方向（1: 正、-1: 負）

    void Awake()
    {
        mainCam = Camera.main;
        startRotation = transform.rotation;
    }

    void Update()
    {
        var mouse = Mouse.current;
        var touchscreen = Touchscreen.current;

        bool clicked = false;
        Vector2 screenPos = Vector2.zero;

        // マウス入力（エディタ・スタンドアロン）
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            clicked = true;
            screenPos = mouse.position.ReadValue();
        }

        // タッチ入力（モバイル）
        if (touchscreen != null && touchscreen.touches.Count > 0)
        {
            var touch = touchscreen.touches[0];
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                clicked = true;
                screenPos = touch.position.ReadValue();
            }
        }

        if (clicked)
        {
            // クリック位置からレイキャスト
            var ray = mainCam.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out var hit, 100f, mirrorLayer) && hit.transform == transform)
            {
                // 鏡をクリックしたら指定角度だけ回転
                RotateByAngle(rotationAnglePerClick);
            }
        }
    }

    void RotateByAngle(float angle)
    {
        // 回転方向を適用
        currentRotationY += angle * rotationDirection;
        
        // 制限に到達したら方向を反転
        if (currentRotationY >= maxAngle)
        {
            currentRotationY = maxAngle;
            rotationDirection = -1; // 逆方向へ
        }
        else if (currentRotationY <= minAngle)
        {
            currentRotationY = minAngle;
            rotationDirection = 1; // 正方向へ
        }
        
        // 開始回転を基準に新しい回転を適用
        transform.rotation = startRotation * Quaternion.Euler(0f, currentRotationY, 0f);
    }
}
