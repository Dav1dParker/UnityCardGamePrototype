using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using _CardGamePrototype.Scripts.View;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _CardGamePrototype.Scripts.Input
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class CardDragHandler : MonoBehaviour
    {
        private RectTransform _parentRect;
        private bool _isDragging;
        private Vector2 _offset;
        private Canvas _canvas;
        private Transform _startParent;
        private readonly List<CardView> _dragGroup = new();
        private readonly Dictionary<CardView, Vector2> _localOffsets = new();
        private RectTransform _dragContainer;
        private readonly Dictionary<CardView, Image> _cardImages = new();
        private Color _originalColor;


        public void Begin(Vector2 pointer, Canvas canvas)
        {
            var card  = GetComponent<CardView>();
            var stack = card.Stack;
            if (stack == null) return;

            _canvas = canvas;
            _dragGroup.Clear();
            _localOffsets.Clear();
            
            var stackRect = (RectTransform)stack.transform;
            int start = card.transform.GetSiblingIndex();
            if (stack.Type == Core.StackType.Tableau)
            {
                int n = stackRect.childCount;
                for (int i = start; i < n; i++)
                {
                    var v = stackRect.GetChild(i).GetComponent<CardView>();
                    if (v != null) _dragGroup.Add(v);
                }
            }
            else
            {
                _dragGroup.Add(card);
            }

            _startParent = stack.transform;
            
            var parentRect = (RectTransform)stackRect.parent;

            _dragContainer = new GameObject("DragContainer", typeof(RectTransform)).GetComponent<RectTransform>();
            _dragContainer.SetParent(parentRect, false);
            _dragContainer.anchorMin = stackRect.anchorMin;
            _dragContainer.anchorMax = stackRect.anchorMax;
            _dragContainer.pivot     = stackRect.pivot;
            _dragContainer.sizeDelta = stackRect.sizeDelta;
            _dragContainer.anchoredPosition = stackRect.anchoredPosition;
            _dragContainer.SetSiblingIndex(parentRect.childCount - 1);

            foreach (var v in _dragGroup)
            {
                var rt = (RectTransform)v.transform;
                _localOffsets[v] = rt.anchoredPosition;
            }
            foreach (var v in _dragGroup)
            {
                var rt = (RectTransform)v.transform;
                rt.SetParent(_dragContainer, false);
                rt.anchoredPosition = _localOffsets[v];
                var g = v.GetComponent<CanvasGroup>() ?? v.gameObject.AddComponent<CanvasGroup>();
                g.blocksRaycasts = false;
            }
            
            _cardImages.Clear();
            foreach (var v in _dragGroup)
            {
                var img = v.GetComponent<Image>();
                if (img != null)
                {
                    _cardImages[v] = img;
                    _originalColor = img.color;
                    img.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f);
                }
            }

            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, pointer, null, out var local);
            _offset = _dragContainer.anchoredPosition - local;

            _isDragging = true;
        }

        private void Update()
        {
            if (!_isDragging) return;

            var parentRect = (RectTransform)_dragContainer.parent;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                Mouse.current.position.ReadValue(),
                null,
                out var local);

            _dragContainer.anchoredPosition = local + _offset;
            
            if (_isDragging)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _canvas.transform as RectTransform,
                    Mouse.current.position.ReadValue(),
                    null, out var pointerLocal);
                _dragContainer.anchoredPosition = pointerLocal + _offset;
                
                var data = new PointerEventData(EventSystem.current)
                {
                    position = Mouse.current.position.ReadValue()
                };
                var hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(data, hits);

                StackView target = null;
                foreach (var h in hits)
                {
                    target = h.gameObject.GetComponent<StackView>() ?? h.gameObject.GetComponentInParent<StackView>();
                    if (target) break;
                }

                bool canDrop = false;
                if (target)
                {
                    switch (target.Type)
                    {
                        case Core.StackType.Foundation:
                            canDrop = _dragGroup.Count == 1;
                            break;
                        case Core.StackType.Tableau:
                            if (target.transform.childCount == 0)
                                canDrop = true;
                            else
                            {
                                var hitCard = hits[0].gameObject.GetComponent<CardView>() ??
                                              hits[0].gameObject.GetComponentInParent<CardView>();
                                if (hitCard && hitCard.Stack == target)
                                {
                                    int index = hitCard.transform.GetSiblingIndex();
                                    int last = target.transform.childCount - 1;
                                    canDrop = index == last;
                                }
                            }
                            break;
                    }
                }
                
                float alpha = canDrop ? 1f : 0.5f;

                foreach (var img in _cardImages.Values)
                {
                    var c = _originalColor;
                    c.a = alpha;
                    img.color = c;
                }

            }

        }


        public void End(Vector2 pointer)
        {
            if (!_isDragging) return;
            _isDragging = false;

            var data = new PointerEventData(EventSystem.current) { position = pointer };
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hits);

            var source = _startParent.GetComponent<StackView>();
            StackView target = null;
            foreach (var h in hits)
            {
                target = h.gameObject.GetComponent<StackView>() ?? h.gameObject.GetComponentInParent<StackView>();
                if (target != null) break;
            }

            var valid = false;

            if (target != null)
            {
                switch (target.Type)
                {
                    case Core.StackType.Deck:
                        valid = false;
                        break;

                    case Core.StackType.Foundation:
                        valid = _dragGroup.Count == 1;
                        break;

                    case Core.StackType.Tableau:
                        if (target.transform.childCount == 0)
                        {
                            valid = true;
                        }
                        else
                        {
                            var hitCard = hits[0].gameObject.GetComponent<CardView>() ??
                                          hits[0].gameObject.GetComponentInParent<CardView>();

                            if (hitCard != null && hitCard.Stack == target)
                            {
                                int index = hitCard.transform.GetSiblingIndex();
                                int lastIndex = target.transform.childCount - 1;
                                valid = index == lastIndex;
                            }
                        }
                        break;
                }
            }

            var destination = valid ? target : source;

            source.RemoveCards(_dragGroup);

            foreach (var v in _dragGroup)
            {
                var g = v.GetComponent<CanvasGroup>();
                if (g != null) g.blocksRaycasts = true;
                destination.AddCard(v);
            }

            destination.UpdateLayout();
            if (destination != source)
                source.UpdateLayout();
            
            foreach (var img in _cardImages.Values)
                img.color = _originalColor;
            _cardImages.Clear();

            Destroy(_dragContainer.gameObject);
            _dragGroup.Clear();
            _localOffsets.Clear();
        }


        
    }
}
