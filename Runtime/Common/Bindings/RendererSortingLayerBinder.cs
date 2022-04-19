using System.Linq;
using UnityEngine;

namespace Common.Bindings
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class RendererSortingLayerBinder : MonoBehaviour
    {
#pragma warning disable 649
        [ConditionalField("_setParentCanvas")] [SerializeField]
        private Canvas _parentCanvas;
#pragma warning restore 649

        private Renderer _mesh;

        private void SetCanvasLayer()
        {
            if (_parentCanvas != null)
            {
                SetCanvasParameters(_mesh);

                var children = _mesh.GetComponentsInChildren<Renderer>();
                if (children != null)
                    foreach (var child in children)
                        SetCanvasParameters(child);
            }
        }

        private void SetCanvasParameters(Renderer mesh)
        {
            mesh.sortingLayerName = _parentCanvas.sortingLayerName;
            mesh.sortingOrder = _parentCanvas.sortingOrder;
        }

        private void OnEnable()
        {
            SetCanvasLayer();
        }

        private void Awake()
        {
            if (_mesh == null)
                _mesh = GetComponent<Renderer>();

            if (_parentCanvas == null)
                _parentCanvas = GetComponentsInParent(typeof(Canvas), true).Cast<Canvas>().FirstOrDefault();

            SetCanvasLayer();
        }
    }
}