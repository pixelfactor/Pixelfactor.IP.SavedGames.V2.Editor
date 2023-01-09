using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    [ExecuteInEditMode]
    public class HideUnitWhenDocked : MonoBehaviour
    {
        private bool? isDocked = null;

        void Update()
        {
            var isDockedNow = this.GetComponentInParent<EditorHangarBay>() != null;
            if (isDockedNow != isDocked)
            {
                foreach (var meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.enabled = isDockedNow != true;
                }

                this.isDocked = isDockedNow;
            }
        }
    }
}
