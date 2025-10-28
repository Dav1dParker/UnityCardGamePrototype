using _CardGamePrototype.Scripts.Core;
using UnityEngine;

namespace _CardGamePrototype.Scripts.View
{
    public sealed class CardView : MonoBehaviour
    {
        public Card Card { get; private set; }
        public StackView Stack { get; set; }

        public void Bind(Card card)
        {
            Card = card;
        }
    }
}
