using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEditor;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Utilities
{
    [ExecuteInEditMode]
    public class SyncScaleWithUnitRadius : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            var unit = this.GetComponentInParent<EditorUnit>();
            if (unit != null && unit.Radius >= 0.0f)
            {
                this.transform.localScale = new Vector3(unit.Radius * 2.0f, unit.Radius * 2.0f, unit.Radius * 2.0f);
            }
        }
    }
}
