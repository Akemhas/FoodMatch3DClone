using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FailUI : MonoBehaviour
{
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private GameObject askPanel;
    [SerializeField] private Transform failNotification;
    [SerializeField] private Transform failPanel;

    [SerializeField] private Button reviveButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private float notificationDuration = 1.5f;

    private Vector3 topPosition;
    private Vector3 middlePosition;
    private Vector3 bottomPosition;

    void Awake()
    {
        Camera mainCam = Camera.main;
        topPosition = mainCam.ViewportToScreenPoint(new Vector3(0.5f, 1.2f, 0));
        middlePosition = mainCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        bottomPosition = mainCam.ViewportToScreenPoint(new Vector3(0.5f, -.2f, 0));
    }

    private void Start()
    {
        exitButton.onClick.AddListener(LoadMainMenu);
        reviveButton.onClick.AddListener(Revive);
        homeButton.onClick.AddListener(LoadMainMenu);
    }

    public void Fail(bool showReviveButton)
    {
        blockPanel.gameObject.SetActive(true);
        failNotification.gameObject.SetActive(true);
        exitButton.interactable = false;
        Sequence seq = DOTween.Sequence();
        seq.Append(failNotification.DOMove(middlePosition, notificationDuration * .5f).From(topPosition)).SetEase(Ease.InOutSine);
        seq.AppendInterval(1f);
        seq.Append(failNotification.DOMove(bottomPosition, notificationDuration * .5f).From(middlePosition));
        seq.SetLink(failNotification.gameObject);
        seq.OnComplete(() =>
        {
            failNotification.gameObject.SetActive(false);
            OpenPanel(showReviveButton);
        });
    }

    private void OpenPanel(bool showReviveButton)
    {
        failPanel.gameObject.SetActive(true);
        reviveButton.gameObject.SetActive(showReviveButton);
        homeButton.gameObject.SetActive(!showReviveButton);
        askPanel.SetActive(showReviveButton);
        failPanel.DOScale(1, 0.25f).From(0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            exitButton.interactable = true;
        }).SetLink(failPanel.gameObject);
    }

    [ContextMenu("Close Panel")]
    private void ClosePanel()
    {
        failNotification.gameObject.SetActive(false);
        exitButton.interactable = false;
        failPanel.DOScale(0, 0.25f).SetEase(Ease.InCirc).OnComplete(() => { failPanel.gameObject.SetActive(false); }).SetLink(failPanel.gameObject);
        blockPanel.SetActive(false);
        homeButton.gameObject.SetActive(false);
        reviveButton.gameObject.SetActive(false);
        askPanel.gameObject.SetActive(false);
    }

    private void Revive()
    {
        StageController.Instance.ReviveGame();
        ClosePanel();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
