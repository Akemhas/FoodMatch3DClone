using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject blockPanel;
    [SerializeField] private Transform panelTransform;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button hintButton;
    private bool isOpen;

    private void Start()
    {
        homeButton.onClick.AddListener(LoadMainMenu);
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        hintButton.onClick.AddListener(Hint);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        SetButtonInteractables(false);
    }

    private void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private void OnSettingsButtonClicked()
    {
        if (StageController.CurrentStage == GameStage.End) return;
        TogglePanel(!isOpen);
    }
    private void TogglePanel(bool targetStatus)
    {
        isOpen = targetStatus;
        blockPanel.gameObject.SetActive(isOpen);
        if (isOpen) panelTransform.gameObject.SetActive(true);
        panelTransform.DOScale(isOpen ? 1 : 0, .3f).From(isOpen ? 0.2f : 1f).SetEase(isOpen ? Ease.OutBack : Ease.InCirc)
        .SetLink(panelTransform.gameObject).OnComplete(() =>
        {
            SetButtonInteractables(isOpen);
            if (!isOpen)
            {
                panelTransform.gameObject.SetActive(false);
            }
        });
        if (targetStatus) Pause();
        else StageController.Instance.Resume();
    }

    private void Hint()
    {
        ItemSlotManager.Instance.ClearHint();
        TogglePanel(false);
    }

    private void Resume()
    {
        TogglePanel(false);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Pause()
    {
        StageController.Instance.PauseGame();
    }

    private void SetButtonInteractables(bool status)
    {
        homeButton.interactable = status;
        resumeButton.interactable = status;
        hintButton.interactable = status;
        restartButton.interactable = status;
    }
}
