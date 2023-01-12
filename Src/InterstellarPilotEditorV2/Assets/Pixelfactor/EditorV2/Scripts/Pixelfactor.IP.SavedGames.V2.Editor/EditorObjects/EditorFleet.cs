using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects.FleetOrders.OrderTypes;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects
{
    public class EditorFleet : MonoBehaviour
    {
        public int Id = -1;

        public EditorFaction Faction;

        /// <summary>
        /// Optional home base. Should be the transform of a unit or a child transform of a sector
        /// </summary>
        public Transform HomeBase;

        /// <summary>
        /// When true the parent faction won't' try to control
        /// </summary>
        public bool ExcludeFromFactionAI;

        /// <summary>
        /// Use this to give the ships a custom name
        /// </summary>
        public string Designation;

        void OnDrawGizmosSelected()
        {
            var isSelected = SelectionHelper.IsSelected(this);
            if (isSelected || SelectionHelper.IsSelected(this.transform.parent) || SelectionHelper.IsSelected(this.transform.parent?.parent))
            {
                DrawString.Draw(this.gameObject.name, this.transform.position, Color.white);

                if (SceneView.lastActiveSceneView.size < 5000.0f)
                {
                    var orders = this.GetComponentsInChildren<EditorFleetOrderBase>();
                    if (orders.Length > 0)
                    {
                        var ordersText = orders[0].name;
                        if (orders.Length > 1)
                        {
                            ordersText += $" +{orders.Length - 1} more";
                        }

                        DrawString.Draw(ordersText, this.transform.position, Color.white, null, new Vector3(0.0f, -20.0f, 0.0f));
                    }
                }
            }
        }

        /// <summary>
        /// An optional transform of where the editor should place fleet orders. The transform should be a child of this object
        /// </summary>
        [Tooltip("An optional transform of where the editor should place fleet orders. The transform should be a child of this object")]
        public Transform EditorOrdersRoot = null;

        public Transform GetOrdersRoot()
        {
            if (this.EditorOrdersRoot != null)
            {
                return this.EditorOrdersRoot;
            }

            return this.transform;
        }
    }
}
