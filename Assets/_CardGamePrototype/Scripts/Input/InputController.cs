using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using _CardGamePrototype.Scripts.View;

namespace _CardGamePrototype.Scripts.Input
{
    public sealed class InputController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        private CardControls controls;
        private CardDragHandler current;

        private void Awake()
        {
            controls = new CardControls();
            if (canvas == null)
                canvas = FindFirstObjectByType<Canvas>();
        }

        private void OnEnable()
        {
            controls.Enable();
            controls.Pointer.Click.performed += OnPointerDown;
            controls.Pointer.Click.canceled  += OnPointerUp;
        }

        private void OnDisable()
        {
            controls.Pointer.Click.performed -= OnPointerDown;
            controls.Pointer.Click.canceled  -= OnPointerUp;
            controls.Disable();
        }

        private void OnPointerDown(InputAction.CallbackContext ctx)
        {
            var pointer = Mouse.current.position.ReadValue();
            var data = new PointerEventData(EventSystem.current) { position = pointer };
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hits);

            foreach (var h in hits)
            {
                var card = h.gameObject.GetComponent<CardView>();
                if (card == null) continue;

                var drag = card.GetComponent<CardDragHandler>();
                if (drag == null) continue;

                drag.Begin(pointer, canvas);
                current = drag;
                break;
            }
        }

        private void OnPointerUp(InputAction.CallbackContext ctx)
        {
            if (current == null) return;
            current.End(Mouse.current.position.ReadValue());
            current = null;
        }
    }
}