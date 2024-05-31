using Romix.Model;
using Romix.Utils;
using UnityEngine;

namespace Romix.Managers
{

    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private DifficultyData easy;
        [SerializeField] private DifficultyData normal;
        [SerializeField] private DifficultyData hard;
        [SerializeField] private CanvasGroup retryScreen;
        [SerializeField] private CanvasGroup winScreen;
        [SerializeField] private float screenTransitionDuration = 0.75f;
        [SerializeField] private Board board;
        public static GameplayManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Setup();
            board.StartMatch();
        }

        private void Setup()
        {
            // Set difficulty
            switch (GameManager.Instance.SelectedDifficulty)
            {
                case DifficultyEnum.Easy:
                    board.Setup(easy);
                    break;
                default:
                case DifficultyEnum.Normal:
                    board.Setup(normal);
                    break;
                case DifficultyEnum.Hard:
                    board.Setup(hard);
                    break;
            }

            board.GameWon += () => ShowEndOfGameScreen(win: true);
        }

        private void ShowEndOfGameScreen(bool win)
        {
            if (win)
            {
                winScreen.gameObject.SetActive(true);
                StartCoroutine(winScreen.FadeIn(screenTransitionDuration));
            }
            else
            {
                retryScreen.gameObject.SetActive(true);
                StartCoroutine(retryScreen.FadeIn(screenTransitionDuration));
            }
        }



        public void RestartMatch()
        {
            StartCoroutine(retryScreen.FadeIn(screenTransitionDuration, () =>
            {
                retryScreen.gameObject.SetActive(false);
                Setup();
                board.StartMatch();
            }));
        }
    }
}