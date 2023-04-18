using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuccessUI : MonoBehaviour
{
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private Transform panel;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button playButton;

    private void Start()
    {
        homeButton.onClick.AddListener(LoadMainMenu);
        playButton.onClick.AddListener(LoadGameScene);
    }

    public void Success()
    {
        blockPanel.gameObject.SetActive(true);
        homeButton.interactable = false;
        OpenPanel();
    }

    private void OpenPanel()
    {
        panel.gameObject.SetActive(true);
        panel.DOScale(1, 0.25f).From(0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            homeButton.interactable = true;
        }).SetLink(panel.gameObject);
    }

    [ContextMenu("Close Panel")]
    private void ClosePanel()
    {
        homeButton.interactable = false;
        panel.DOScale(0, 0.25f).SetEase(Ease.InCirc).OnComplete(() => { panel.gameObject.SetActive(false); }).SetLink(panel.gameObject);
        blockPanel.SetActive(false);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
