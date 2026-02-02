// QteSwipeHandler.cs – Простая система QTE свайпа

using System;
using UnityEngine;
using UnityEngine.UI;

namespace QteSystem.TestQte
{
    public class QteSwipeHandler : MonoBehaviour
    {
        public RectTransform swipeZone;
        public Image arrowImage;
        public float minSwipeDistance = 50f;
        public float angleThreshold = 30f;
        public SwipeDirection expectedDirection = SwipeDirection.Right;

        private Vector2 _startPos;
        private bool _tracking = false;

        public Action<bool> OnSwipeResult;

        void Start()
        {
            SetArrowDirection(expectedDirection);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUIZone(Input.mousePosition))
                {
                    _tracking = true;
                    _startPos = Input.mousePosition;
                }
            }

            if (_tracking && Input.GetMouseButtonUp(0))
            {
                _tracking = false;
                Vector2 endPos = Input.mousePosition;
                Vector2 delta = endPos - _startPos;

                if (delta.magnitude < minSwipeDistance)
                {
                    OnSwipeResult?.Invoke(false);
                    return;
                }

                Vector2 dir = delta.normalized;
                Vector2 expected = GetVector(expectedDirection);
                float angle = Vector2.Angle(dir, expected);

                bool success = angle <= angleThreshold;
                OnSwipeResult?.Invoke(success);
            }
        }

        bool IsPointerOverUIZone(Vector2 pos)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(swipeZone, pos);
        }

        Vector2 GetVector(SwipeDirection dir)
        {
            return dir switch
            {
                SwipeDirection.Up => Vector2.up,
                SwipeDirection.Down => Vector2.down,
                SwipeDirection.Left => Vector2.left,
                _ => Vector2.right,
            };
        }

        void SetArrowDirection(SwipeDirection dir)
        {
            float angle = dir switch
            {
                SwipeDirection.Up => 0f,
                SwipeDirection.Right => -90f,
                SwipeDirection.Down => 180f,
                SwipeDirection.Left => 90f,
                _ => 0f,
            };
            arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        public enum SwipeDirection
        {
            Up,
            Down,
            Left, 
            Right
        }
    }
}
