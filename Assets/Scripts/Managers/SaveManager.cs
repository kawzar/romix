using Romix.Model;
using UnityEngine;

namespace Romix.Managers
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private const string UNVEILED_KEY = "UNVEILED";
        [SerializeField] private const string MATCHED_KEY = "MATCHED";
        public static SaveManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public HighScoreData GetHighScore()
        {
            int unveiled = PlayerPrefs.GetInt(UNVEILED_KEY);
            int matched = PlayerPrefs.GetInt(MATCHED_KEY);

            return new HighScoreData { Matched = matched, Revealed = unveiled };
        }
        
        public void SaveHighScore(HighScoreData data)
        {
            if(PlayerPrefs.GetInt(UNVEILED_KEY) != 0 && data.Revealed > PlayerPrefs.GetInt(UNVEILED_KEY)) return;
            
            PlayerPrefs.SetInt(UNVEILED_KEY, data.Revealed);
            PlayerPrefs.SetInt(MATCHED_KEY, data.Matched);
        }
    }
}