using System;
using Romix.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Romix.Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Animator animator;
        [SerializeField] private Button button;
        [SerializeField] private AudioClip flipAudioClip;
        
        private CardTypeEnum type;
        private Sprite backSprite;
        private Sprite iconSprite;
        private bool isShowing = false;

        public CardTypeEnum Type => type;
        public Button Button => button;
        public event Action<Card> CardRevealed;

        public void Initialize(CardTypeEnum cardType, Sprite back, Sprite icon)
        {
            type = cardType;
            backSprite = back;
            iconSprite = icon;
            image.sprite = backSprite;
            button.onClick.AddListener(OnCardClicked);
        }

        public void OnCardClicked()
        {
            if (!isShowing)
            {
                ToggleShow();
            }
        }

        public void ToggleShow()
        {
            isShowing = !isShowing;
            image.sprite = isShowing ? iconSprite : backSprite;
            AudioManager.Instance.PlaySound(flipAudioClip);
            
            if (isShowing)
            {
                animator.Play("Select");
                CardRevealed?.Invoke(this);
            }
            else
            {
                animator.Play("Unselect");
            }
        }

        public bool Equals(Card another)
        {
            return type == another.Type;
        }

        public void PlayCorrectAnimation()
        {
            animator.Play("Correct");
        }

        public void PlayWrongAnimation()
        {
            animator.Play("Wrong");
        }

        private void OnEnable()
        {
            animator.Play("PairSpawn");
        }
    }
}