using UnityEngine;

namespace _Game.Logic.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SelectionBox : MonoBehaviour
    {
        private const int INDEX_SELECTED_MOUSE_BUTTON = 0;

        [SerializeField] private RectTransform _selectedBox;

        private Vector2 _starPosition;
        private Vector2 _endPosition;

        private void Start()
        {
            Hide();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(INDEX_SELECTED_MOUSE_BUTTON))
            {
                _starPosition = Input.mousePosition;
                Show();
            }

            if (Input.GetMouseButton(INDEX_SELECTED_MOUSE_BUTTON))
            {
                UpdateSelectionBox();
            }

            if (Input.GetMouseButtonUp(INDEX_SELECTED_MOUSE_BUTTON))
            {
                Hide();
            }
        }


        private void UpdateSelectionBox()
        {
            _endPosition = Input.mousePosition;
            Vector2 min = new Vector2(Mathf.Min(_endPosition.x, _starPosition.x),
                Mathf.Min(_endPosition.y, _starPosition.y));
            Vector2 max = new Vector2(Mathf.Max(_endPosition.x, _starPosition.x),
                Mathf.Max(_endPosition.y, _starPosition.y));

            _selectedBox.position = min;
            _selectedBox.sizeDelta = max - min;
        }

        private void Show()
        {
            _selectedBox.gameObject.SetActive(true);
        }

        private void Hide()
        {
            _selectedBox.gameObject.SetActive(false);
        }
    }
}