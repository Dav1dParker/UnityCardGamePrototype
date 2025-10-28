using System.Collections.Generic;
using UnityEngine;
using _CardGamePrototype.Scripts.Core;
using _CardGamePrototype.Scripts.View;

namespace _CardGamePrototype.Scripts.Logic
{
    public sealed class GameHistory : MonoBehaviour
    {
        private readonly Stack<MoveRecord> _moves = new();

        public void Push(MoveRecord move) => _moves.Push(move);

        public void Undo()
        {
            if (_moves.Count == 0) return;
            var m = _moves.Pop();

            m.Target.RemoveCards(m.Cards);
            
            var pairs = new List<(CardView card, int idx)>(m.Cards.Count);
            for (int i = 0; i < m.Cards.Count; i++) pairs.Add((m.Cards[i], m.SourceIndices[i]));
            pairs.Sort((a, b) => a.idx.CompareTo(b.idx));

            foreach (var p in pairs)
                m.Source.InsertCardAt(p.card, p.idx);

            m.Source.UpdateLayout();
            m.Target.UpdateLayout();
        }

    }
}