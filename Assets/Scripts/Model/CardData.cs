using UnityEngine;

namespace Romix.Model
{
    [CreateAssetMenu(fileName = "Card", menuName = "Model/Card", order = 1)]
    public class CardData : ScriptableObject
    {
        [SerializeField]
        public CardTypeEnum Type;

        [SerializeField]
        public Sprite Sprite;
    }
}