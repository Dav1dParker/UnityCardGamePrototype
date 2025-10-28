using UnityEngine;

namespace _CardGamePrototype.Scripts.Logic
{
    public sealed class UndoButton : MonoBehaviour
    {
        [SerializeField] private GameHistory _history;
        public void OnClick() => _history?.Undo();
    }
}