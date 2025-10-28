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
        private RectTransform rect;
        private bool dragging;
        private Vector2 offset;
        private Canvas canvas;
        private Transform startParent;
        private readonly List<CardView> dragGroup = new();

        private void Awake() => rect = GetComponent<RectTransform>();

        public void Begin(Vector2 pointer, Canvas c)
        {
            var card = GetComponent<CardView>();
            var stack = card.Stack;
            if (stack == null)
                return;

            canvas = c;
            dragGroup.Clear();
            if (stack.Type == Core.StackType.Tableau)
            {
                bool include = false;
                foreach (Transform child in stack.transform)
                {
                    var v = child.GetComponent<CardView>();
                    if (v == null) continue;
                    if (v == card) include = true;
                    if (include) dragGroup.Add(v);
                }
            }
            else
            {
                dragGroup.Add(card);
            }


            startParent = stack.transform;

            foreach (var v in dragGroup)
            {
                v.transform.SetParent(canvas.transform, true);
                var g = v.GetComponent<CanvasGroup>() ?? v.gameObject.AddComponent<CanvasGroup>();
                g.blocksRaycasts = false;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, pointer, null, out var local);
            offset = rect.anchoredPosition - local;
            dragging = true;
        }

        public void End(Vector2 pointer)
        {
            if (!dragging) return;
            dragging = false;

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
                target = startParent.GetComponent<StackView>();

            var source = startParent.GetComponent<StackView>();
            source?.RemoveCards(dragGroup);

            foreach (var v in dragGroup)
            {
                var g = v.GetComponent<CanvasGroup>();
                if (g != null) g.blocksRaycasts = true;
                target.AddCard(v);
            }

            dragGroup.Clear();
        }

        private void Update()
        {
            if (!dragging) return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Mouse.current.position.ReadValue(),
                null,
                out var local);

            var pos = local + offset;
            foreach (var v in dragGroup)
                ((RectTransform)v.transform).anchoredPosition = pos;
        }
    }
}
