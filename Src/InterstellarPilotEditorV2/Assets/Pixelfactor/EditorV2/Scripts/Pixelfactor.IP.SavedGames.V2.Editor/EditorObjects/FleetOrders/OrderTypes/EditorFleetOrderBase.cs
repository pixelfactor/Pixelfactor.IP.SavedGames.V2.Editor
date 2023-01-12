using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes
{
    [ExecuteInEditMode]
    public class EditorFleetOrderBase : MonoBehaviour
    {
        public virtual Vector3? GetTargetPosition()
        {
            return null;
        }

        void OnDrawGizmosSelected()
        {
            var targetPosition = GetTargetPosition();

            if (targetPosition.HasValue)
            {
                var fleet = this.GetComponentInParent<EditorFleet>();
                if (fleet != null)
                {
                    if (!SelectionHelper.IsSelected(fleet))
                    {
                        return;
                    }

                    if (SceneView.lastActiveSceneView.size < 5000.0f)
                    {
                        Handles.color = Color.blue;
                        Handles.DrawLine(this.transform.position, targetPosition.Value, 4.0f);
                        Handles.Label(targetPosition.Value, $"{this.name}");
                        Handles.color = Color.white;
                    }
                }
            }
        }
    }
}
