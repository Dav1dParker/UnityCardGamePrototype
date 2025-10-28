using System;
using NUnit.Framework.Internal.Builders;
using UnityEngine;

namespace _CardGamePrototype.Scripts.Core
{
    [Serializable]
    public sealed class Card
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
