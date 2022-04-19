using UnityEngine;
using UnityEngine.UI;

namespace Common.Bindings
{
    public class ChangeGraphicAlphaOnObjectActivation : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private Graphic _widget;
        [SerializeField] private float _onActive = 1;
        [SerializeField] private float _onInactive;
#pragma warning restore 649

        private void OnEnable()
        {
            if (_widget != null)
            {
                var color = _widget.color;
                color.a = _onActive;
                _widget.color = color;
            }
        }

        private void OnDisable()
        {
            if (_widget != null)
            {
                var color = _widget.color;
                color.a = _onInactive;
                _widget.color = color;
            }
        }
    }
}