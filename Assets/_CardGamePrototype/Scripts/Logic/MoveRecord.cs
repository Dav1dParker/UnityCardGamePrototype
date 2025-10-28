using System.Collections.Generic;
using _CardGamePrototype.Scripts.Core;
using _CardGamePrototype.Scripts.View;

namespace _CardGamePrototype.Scripts.Logic
{
    public sealed class MoveRecord
    {
        public StackView Source;
        public StackView Target;
        public List<CardView> Cards;
        public List<int> SourceIndices;
    }
}