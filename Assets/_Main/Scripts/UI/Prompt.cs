using UnityEngine;
using DG.Tweening;

public abstract class Prompt : MonoBehaviour
{
    [SerializeField] private Transform promptPanel;

    public void OpenPrompt()
    {
        promptPanel.gameObject.SetActive(true);
        promptPanel.DOScale(1, .25f).From(0).SetLink(promptPanel.gameObject);
    }

    public void ClosePrompt()
    {
        promptPanel.DOScale(0, .25f).SetLink(promptPanel.gameObject).OnComplete(() =>
        {
            promptPanel.gameObject.SetActive(false);
        });
    }
}
