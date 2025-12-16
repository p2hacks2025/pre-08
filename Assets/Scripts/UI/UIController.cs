using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject objectPanel;
    [SerializeField] private GameObject openButton;

    void Start()
    {
        //初期状態ではオブジェクトパネルを非表示、開くボタンを表示
        if (objectPanel != null) objectPanel.SetActive(false);
        if (openButton != null) openButton.SetActive(true);
    }
    
    public void ToggleObjectPanel()
    {
        //UIの切替
        if (objectPanel != null && openButton != null)
        {
            bool isActive = objectPanel.activeSelf;
            objectPanel.SetActive(!isActive);
            openButton.SetActive(isActive);
        }
    }
}
