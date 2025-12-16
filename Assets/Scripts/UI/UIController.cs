using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject objectPanel;   //オブジェクトパネル
    [SerializeField] private GameObject openButton;    //開くボタン
    [SerializeField] private float slideSpeed = 500f;  //スライド速度
    
    private Vector2 panelOriginalPosition;             //パネルの元の位置

    void Start()
    {
        //初期状態でパネルの元の位置を保存し、画面外に配置
        if (objectPanel != null) 
        {
            RectTransform panelRect = objectPanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                panelOriginalPosition = panelRect.anchoredPosition;
                //スライドアウト後の位置に配置
                panelRect.anchoredPosition = panelOriginalPosition + new Vector2(0, -panelRect.rect.height - 100);
            }
        }
        if (openButton != null) openButton.SetActive(true);
    }
    
    public void SlideOutPanel()
    {
        //パネルを下にスライドアウトして非表示にする
        if (objectPanel != null) StartCoroutine(SlideOutCoroutine());
    }
    public void SlideInPanel()
    {
        //パネルを上にスライドインして表示する
        if (objectPanel != null) StartCoroutine(SlideInCoroutine());
    }
    
    private IEnumerator SlideOutCoroutine()
    {
        RectTransform panelRect = objectPanel.GetComponent<RectTransform>();
        if (panelRect == null) yield break;
        
        //開始位置と終了位置を計算
        Vector2 startPos = panelRect.anchoredPosition;
        Vector2 endPos = startPos + new Vector2(0, -panelRect.rect.height - 100);
        
        //移動距離からアニメーション時間を算出
        float distance = Vector2.Distance(startPos, endPos);
        float duration = distance / slideSpeed;
        float elapsed = 0f;
        
        //パネルを下にスライド
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }
        
        panelRect.anchoredPosition = endPos;
        //開くボタンを表示
        if (openButton != null) openButton.SetActive(true);
    }
    private IEnumerator SlideInCoroutine()
    {
        RectTransform panelRect = objectPanel.GetComponent<RectTransform>();
        if (panelRect == null) yield break;
        
        //開くボタンを非表示
        if (openButton != null) openButton.SetActive(false);
        
        //現在の位置（画面外）から元の位置へスライド
        Vector2 startPos = panelRect.anchoredPosition;
        Vector2 endPos = panelOriginalPosition;
        
        //移動距離からアニメーション時間を算出
        float distance = Vector2.Distance(startPos, endPos);
        float duration = distance / slideSpeed;
        float elapsed = 0f;
        
        //パネルを上にスライド
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelRect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }
        
        panelRect.anchoredPosition = endPos;
    }
}
