using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using _CardGamePrototype.Scripts.View;
using UnityEngine.InputSystem;

namespace _CardGamePrototype.Scripts.Input
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class CardDragHandler : MonoBehaviour
    {
        private RectTransform _rect;
        private bool _isDragging;
        private Vector2 _offset;
        private Canvas _canvas;
        private Transform _startParent;
        private readonly List<CardView> _dragGroup = new();
        
        private readonly Dictionary<CardView, Vector2> _localOffsets = new();

        private void Awake() => _rect = GetComponent<RectTransform>();

        public void Begin(Vector2 pointer, Canvas c)
        {
            var card = GetComponent<CardView>();
            var stack = card.Stack;
            if (stack == null)
                return;

            _canvas = c;
            _dragGroup.Clear();
            if (stack.Type == Core.StackType.Tableau)
            {
                bool include = false;
                foreach (Transform child in stack.transform)
                {
                    var v = child.GetComponent<CardView>();
                    if (v == null) continue;
                    if (v == card) include = true;
                    if (include) _dragGroup.Add(v);
                }
            }
            else
            {
                _dragGroup.Add(card);
            }


            _startParent = stack.transform;

            foreach (var v in _dragGroup)
            {
                var rectV = (RectTransform)v.transform;
                _localOffsets[v] = rectV.anchoredPosition;

                v.transform.SetParent(_canvas.transform, true);
                var g = v.GetComponent<CanvasGroup>() ?? v.gameObject.AddComponent<CanvasGroup>();
                g.blocksRaycasts = false;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, pointer, null, out var local);
            _offset = _rect.anchoredPosition - local;
            _isDragging = true;
        }

        public void End(Vector2 pointer)
        {
            if (!_isDragging) return;
            _isDragging = false;

            var data = new PointerEventData(EventSystem.current) { position = pointer };
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hits);

            StackView target = null;
            foreach (var h in hits)
            {
                target = h.gameObject.GetComponentInParent<StackView>();
                if (target != null) break;
            }

            if (target == null || target.Type == Core.StackType.Deck)
                target = _startParent.GetComponent<StackView>();

            var source = _startParent.GetComponent<StackView>();
            source?.RemoveCards(_dragGroup);

            foreach (var v in _dragGroup)
            {
                var g = v.GetComponent<CanvasGroup>();
                if (g != null) g.blocksRaycasts = true;
                target.AddCard(v);
            }

            _dragGroup.Clear();
            _localOffsets.Clear();
        }

        private void Update()
        {
            if (!_isDragging) return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Mouse.current.position.ReadValue(),
                null,
                out var local);

            var pos = local + _offset;
            foreach (var v in _dragGroup)
            {
                var rectV = (RectTransform)v.transform;
                var baseOffset = _localOffsets.TryGetValue(v, out var off) ? off : Vector2.zero;
                rectV.anchoredPosition = pos + off;
            }

        }
    }
}
