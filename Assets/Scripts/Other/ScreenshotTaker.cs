using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenshotTaker : MonoBehaviour
{
    // 再生して「K」キーを押すと撮影されます
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            CaptureTransparentScreenshot();
        }
    }

    void CaptureTransparentScreenshot()
    {
        Camera cam = GetComponent<Camera>();
        
        // 1. 背景を透明設定にする
        var clearFlags = cam.clearFlags;
        var bakColor = cam.backgroundColor;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0); // 完全透明

        // 2. レンダーテクスチャを用意
        RenderTexture rt = new RenderTexture(512, 512, 24); // 解像度は必要に応じて変更
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(512, 512, TextureFormat.ARGB32, false);

        // 3. 撮影
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        screenShot.Apply();

        // 4. ファイルに保存
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "MirrorIcon_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        System.IO.File.WriteAllBytes(Application.dataPath + "/" + filename, bytes);
        
        // 5. 後片付け
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        cam.clearFlags = clearFlags;
        cam.backgroundColor = bakColor;

        Debug.Log("保存しました: Assets/" + filename);
    }
}