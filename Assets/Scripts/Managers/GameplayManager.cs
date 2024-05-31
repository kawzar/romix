using Romix.Model;
using UnityEngine;

namespace Romix.Managers
{

    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private DifficultyData easy;
        [SerializeField] private DifficultyData normal;
        [SerializeField] private DifficultyData hard;
        [SerializeField] private float screenTransitionDuration = 0.75f;
        [SerializeField] private Board board;
        [SerializeField] private GameOverScreen gameOverScreen;
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
            string titleText = win ? "You won!" : "Game Over";
            gameOverScreen.Setup(titleText, board.UnveiledCardsAmount, board.MatchedCardsAmount, RestartMatch);
            gameOverScreen.Show(screenTransitionDuration);

            if (win)
            {
                HighScoreData data = new HighScoreData
                {
                    Revealed = board.UnveiledCardsAmount, 
                    Matched = board.MatchedCardsAmount
                };
                
                SaveManager.Instance.SaveHighScore(data);
            }
        }
        
        public void RestartMatch()
        {
            Setup();
            gameOverScreen.Hide(screenTransitionDuration, () =>
            {
                board.StartMatch();
            });
        }
    }
}