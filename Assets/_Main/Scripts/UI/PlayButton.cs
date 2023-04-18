using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(LoadGameScene);
    }

    private void Start()
    {
       playButton.transform.DOScale(1.1f,1.3f).From(1).SetEase(Ease.InOutSine).SetLink(playButton.gameObject).SetLoops(-1,LoopType.Yoyo);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
