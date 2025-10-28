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

        public void AddCard(CardView view)
        {
            _cards.Add(view);
            view.Stack = this;
            view.transform.SetParent(transform, false);
        }

        public void RemoveCards(IEnumerable<CardView> views)
        {
            foreach (var v in views) _cards.Remove(v);
        }

        public void UpdateLayout()
        {
            if (_cards.Count == 0) return;

            float baseOffset = 45f;
            float scale = Mathf.Clamp01(17f / _cards.Count);
            float yOffset = Type == StackType.Tableau ? -(baseOffset * scale) : 0f;

            for (int i = 0; i < _cards.Count; i++)
            {
                var rect = (RectTransform)_cards[i].transform;
                rect.SetSiblingIndex(i);
                rect.anchoredPosition = new Vector2(0f, i * yOffset);
            }
        }


    }
}
