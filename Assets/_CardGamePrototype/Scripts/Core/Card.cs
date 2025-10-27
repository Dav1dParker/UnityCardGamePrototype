using System;
using NUnit.Framework.Internal.Builders;
using UnityEngine;

namespace _CardGamePrototype.Scripts.Core
{
    [Serializable]
    public sealed class Card
    {
        public Guid Id { get; } = Guid.NewGuid();
        
        // For future logic
        public string Suit { get; set; }
        public string Value { get; set; }


        public Card(string suit, string rank)
        {
            Suit = suit;
            Value = rank;
        }
    }
}
