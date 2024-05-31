using Romix.Model;
using UnityEngine;

namespace Romix.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] DifficultyEnum selectedDifficulty = DifficultyEnum.Normal;
        public DifficultyEnum SelectedDifficulty => selectedDifficulty;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}