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
            var move = _moves.Pop();

            move.Target.RemoveCards(move.Cards);

            for (int i = 0; i < move.Cards.Count; i++)
            {
                var card = move.Cards[i];
                card.transform.SetParent(move.Source.transform, false);
                card.transform.SetSiblingIndex(move.SourceIndices[i]);
                card.Stack = move.Source;
            }

            move.Source.UpdateLayout();
            move.Target.UpdateLayout();
        }
    }
}