using System.Collections.Generic;
using UnityEngine;

namespace _CardGamePrototype.Scripts.Core
{
    public sealed class CardStack
    {
        public StackType Type { get;}
        private readonly List<Card> _cards = new();
        
        public IReadOnlyList<Card> Cards => _cards;
        
        public CardStack(StackType type)
        {
            Type = type;
        }

        public void Push(Card card) => _cards.Add(card);

        public Card Pop()
        {
            if (_cards.Count == 0) return null;
            var card = _cards[^1];
            _cards.RemoveAt(_cards.Count - 1);
            return card;
        }
    }
}