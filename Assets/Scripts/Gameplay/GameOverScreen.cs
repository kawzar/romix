using System;
using Romix.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI unveiledText;
    [SerializeField] private TextMeshProUGUI matchedText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button tryAgainButton;
    
    public void Setup(string title, int unveiled, int matched, Action tryAgainAction)
    {
        titleText.SetText(title);
        unveiledText.SetText(unveiled.ToString());
        matchedText.SetText(matched.ToString());
        tryAgainButton.onClick.AddListener(tryAgainAction.Invoke);
    }

    public void Show(float time)
    {
        canvasGroup.gameObject.SetActive(true);
        StartCoroutine(canvasGroup.FadeIn(time));
    }
    
    public void Hide(float time, Action callback)
    {
        StartCoroutine(canvasGroup.FadeOut(time, () =>
        {
            canvasGroup.gameObject.SetActive(false);
            callback?.Invoke();
        }));
    }
}
