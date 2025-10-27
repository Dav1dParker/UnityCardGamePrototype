using _CardGamePrototype.Scripts.View;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _CardGamePrototype.Scripts.Input
{
    public sealed class CardDragHandler : MonoBehaviour
    {
        private  Canvas _canvas;
        private RectTransform _rect;
        private CanvasGroup _group;
        private CardControls _controls;
        private bool _isDragging = false;
        private Vector2 _startPosition;
        private Transform _StartParent;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _group = gameObject.AddComponent<CanvasGroup>();
            _controls = new CardControls();
            
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas == null)
            {
                _canvas = gameObject.AddComponent<Canvas>();
                if (_canvas == null)
                {
                    Debug.LogError("Canvas not found");
                }
            }
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Pointer.Click.performed += ctx => OnClickPerformed();
            _controls.Pointer.Click.canceled += ctx => OnClickCanceled();
        }

        private void OnDisable()
        {
            _controls.Pointer.Click.performed -= ctx => OnClickPerformed();
            _controls.Pointer.Click.canceled -= ctx => OnClickCanceled();
            _controls.Disable();
        }

        private void OnClickPerformed()
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(_rect, _controls.Pointer.Point.ReadValue<Vector2>())) return;
            _startPosition = _rect.anchoredPosition;
            _StartParent = transform.parent;
            _isDragging = true;
            _group.blocksRaycasts = false;
            transform.SetParent(_canvas.transform, true);
        }

        private void OnClickCanceled()
        {
            _isDragging = false;
            _group.blocksRaycasts = true;
            
            var worldPoint = Mouse.current.position.ReadValue();
            var raycastResults = new System.Collections.Generic.List<RaycastResult>();
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = worldPoint
            };
            
            EventSystem.current.RaycastAll(eventData, raycastResults);

            StackView target = null;
            foreach (var result in raycastResults)
            {
                target = result.gameObject.GetComponentInParent<StackView>();
                if (target != null) break;
            }

            if (target != null)
            {
                transform.SetParent(target.transform, false);
                target.AddCard(GetComponent<CardView>());
            }
            else
            {
                transform.SetParent(_StartParent, false);
                _StartParent.GetComponent<StackView>().AddCard(GetComponent<CardView>());
                _rect.anchoredPosition = _startPosition;
            }
        }


        private void Update()
        {
            if (!_isDragging) return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                _controls.Pointer.Point.ReadValue<Vector2>(),
                null, out var pos);
            _rect.anchoredPosition = pos;
        }
        
        
    }
}
