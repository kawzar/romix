using UnityEngine;

namespace Romix.Model
{
    [CreateAssetMenu(fileName = "Difficulty", menuName = "Model/Difficulty", order = 2)]
    public class DifficultyData : ScriptableObject
    {
        public string Name;
        public CardData[] AvailableCards;
        public int AmountOfPairs;
        public float TimeAvailable;
    }
}