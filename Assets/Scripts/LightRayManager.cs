using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class LightRayManager : MonoBehaviour
{
    public float maxDistance = 100f; 
    public int maxReflections = 5;
    public float raySpeed = 10f; // 光線の進む速度 (単位/秒)
    public LayerMask playerLayer; // プレイヤーのレイヤー
    public float lineWidth = 0.1f; // 線の太さ

    private LineRenderer lineRenderer;
    private float elapsedTime = 0f; // 経過時間
    private bool isActive = false; // 発射状態
    private bool isCompleted = false; // 光線が完全に描画されたか
    private bool hasReachedGoal = false; // ゴールに到達したか

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        // 線の太さを設定
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    void Update()
    {
        // プレイヤーをクリック/タップで光線発射
        HandleInput();
        
        if (isActive)
        {
            if (!isCompleted)
            {
                elapsedTime += Time.deltaTime;
            }
            DrawLightPath();
        }
    }

    void HandleInput()
    {
        var mouse = Mouse.current;
        var touchscreen = Touchscreen.current;

        bool isPressed = false;
        Vector2 inputPos = Vector2.zero;

        // マウス入力（エディタ・スタンドアロン）
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            isPressed = true;
            inputPos = mouse.position.ReadValue();
        }
        
        // タッチ入力（モバイル）
        if (touchscreen != null && touchscreen.touches.Count > 0)
        {
            var touch = touchscreen.touches[0];
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                isPressed = true;
                inputPos = touch.position.ReadValue();
            }
        }

        if (isPressed)
        {
            // タップ位置からレイキャスト
            Ray ray = Camera.main.ScreenPointToRay(inputPos);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, playerLayer))
            {
                // プレイヤーをタップした場合、光線を発射
                if (hit.transform == transform)
                {
                    isActive = true;
                    elapsedTime = 0f; // リセット
                    isCompleted = false; // リセット
                    hasReachedGoal = false; // リセット
                }
            }
        }
    }

    void DrawLightPath()
    {
        // Vector3を使用
        Vector3 currentPosition = transform.position;
        
        // 織姫/彦星オブジェクトのZ軸の回転を方向として使用
        // Z軸は2Dトップビューでの回転を表すため、transform.forwardを使います。
        Vector3 currentDirection = transform.forward;
        
        // Z座標は0または一定値に固定されることを前提とする (2D的な動きの制約)

        var points = new List<Vector3>();
        var hitGoal = false; // ゴールに到達したか
        var goalPointIndex = -1; // ゴールの点のインデックス
        points.Add(currentPosition);

        // Raycastingの処理
        for (int i = 0; i < maxReflections; i++)
        {
            // 1. Rayを飛ばす (Physics2DではなくPhysicsを使用)
            // Raycast(開始位置, 方向, 衝突情報格納用, 最大距離)
            // RaycastHit型を使用
            RaycastHit hit; 
            
            // Raycastでは戻り値がbool (当たったかどうか)
            if (Physics.Raycast(currentPosition, currentDirection, out hit, maxDistance))
            {
                // **A. 何かに当たった場合**
                points.Add(hit.point); 

                // 衝突したオブジェクトのタグをチェック
                if (hit.collider.CompareTag("Reflector")) // 3D反射鏡に当たった場合
                {
                    // 2. 反射方向の計算 (Vector3.Reflectを使用)
                    // hit.normal は当たった面の法線ベクトル (Vector3型)
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal).normalized;
                    
                    // 次のレイが衝突地点から正しく出るよう、少しずらす
                    currentPosition = hit.point + currentDirection * 0.01f; 
                    
                    // Z軸の値を一定に保つ (2Dゲームの制約維持)
                    // Z座標をリセットすることで、光がZ軸方向に漏れるのを防ぎます。
                    currentPosition.z = 0f; 

                }
                else if (hit.collider.CompareTag("Goal")) // 3Dゴールに当たった場合
                {
                    hitGoal = true;
                    goalPointIndex = points.Count - 1; // ゴールの点のインデックス（既に追加済み）
                    break;
                }
                else
                {
                    // 反射鏡でもゴールでもない（壁など）に当たった場合
                    break; 
                }
            }
            else
            {
                // **B. 何にも当たらなかった場合**
                points.Add(currentPosition + currentDirection * maxDistance);
                break;
            }
        }

        // 線の描画
        // 時間ベースで表示するポイント数を制限して、ゆっくり進む効果を出す
        float distanceTraveled = elapsedTime * raySpeed;
        
        int visiblePointCount = 0;
        float accumulatedDistance = 0f;
        
        for (int i = 0; i < points.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(points[i], points[i + 1]);
            if (accumulatedDistance + segmentDistance <= distanceTraveled)
            {
                accumulatedDistance += segmentDistance;
                visiblePointCount = i + 2;
            }
            else
            {
                // 部分的に表示するポイントを追加
                float remainingDistance = distanceTraveled - accumulatedDistance;
                Vector3 direction = (points[i + 1] - points[i]).normalized;
                Vector3 partialPoint = points[i] + direction * remainingDistance;
                
                List<Vector3> displayPoints = new List<Vector3>(points.GetRange(0, i + 1));
                displayPoints.Add(partialPoint);
                
                lineRenderer.positionCount = displayPoints.Count;
                lineRenderer.SetPositions(displayPoints.ToArray());
                return;
            }
        }
        
        lineRenderer.positionCount = visiblePointCount;
        if (visiblePointCount > 0)
        {
            lineRenderer.SetPositions(points.GetRange(0, visiblePointCount).ToArray());
        }
        
        // 全ての点が表示されたら完了状態に
        if (visiblePointCount >= points.Count)
        {
            isCompleted = true;
        }
        
        // ゴールの点まで光線が到達した場合のみゴール判定
        if (hitGoal && visiblePointCount > goalPointIndex && !hasReachedGoal)
        {
            Debug.Log("光がゴールに到達しました！");
            hasReachedGoal = true;
        }
    }
}
