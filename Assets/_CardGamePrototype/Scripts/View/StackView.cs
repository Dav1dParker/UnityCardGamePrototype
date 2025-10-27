using System.Collections.Generic;
using _CardGamePrototype.Scripts.Core;
using UnityEngine;

namespace _CardGamePrototype.Scripts.View
{
    public sealed class StackView : MonoBehaviour
    {
        [SerializeField] private StackType type;
        public StackType Type => type;
        private readonly List<CardView> _cards = new();

        public void AddCard(CardView cardView)
        {
            _cards.Add(cardView);
            cardView.transform.SetParent(transform, false);
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            float offset = Type == StackType.Tableau ? -25f : 0f;
            for (int i = 0; i < _cards.Count; i++)
            {
                // log type + offset
                Debug.Log($"StackView UpdateLayout: name={name}, type={Type}, offset={offset}");
                var t = _cards[i].transform as RectTransform;
                t.anchoredPosition = new Vector2(0, i * offset);
            }
        }
    }
}
