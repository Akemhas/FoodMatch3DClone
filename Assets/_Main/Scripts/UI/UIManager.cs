using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using DG.Tweening;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public BonusMultiplier bonusMultiplier;
    [Space]
    [SerializeField] private Image starImagePrefab;
    [Space]
    [SerializeField] private Transform uiStarTransform;
    [SerializeField] private TextMeshProUGUI uiStarCountTMP;

    private Vector3 uiStarPosition;
    private Camera mainCam;
    private ObjectPool<Image> starPool;
    private int starCount;

    private void Awake()
    {
        mainCam = Camera.main;
        uiStarCountTMP.SetText($"{starCount}");
        starPool = new(() => Instantiate(starImagePrefab, transform), x => x.gameObject.SetActive(true), x => x.gameObject.SetActive(false), defaultCapacity: 10, maxSize: 20);
    }

    private void Start()
    {
        uiStarPosition = uiStarTransform.position;
    }

    public void SendStar(Vector3 fromPosition, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SendImageFromToLocation(starPool.Get(), fromPosition, uiStarPosition, i * 0.0618f);
        }
    }

    private void SendImageFromToLocation(Image image, Vector3 worldPos, Vector3 targetPosition, float delay = 0)
    {
        Vector3 convertedPos = mainCam.WorldToScreenPoint(worldPos);
        image.transform.position = convertedPos;
        image.rectTransform.DOMove(targetPosition, .42f).SetLink(image.gameObject).SetDelay(delay).OnComplete(() =>
        {
            starCount++;
            uiStarCountTMP.SetText($"{starCount}");
            starPool.Release(image);
        });
    }
}
