using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools
{
    /// <summary>
    /// Purpose was to avoid accidentally moving child models when a parent unit should have been moved
    /// </summary>
    [ExecuteInEditMode]
    public class ForceNoLocalPosition : MonoBehaviour
    {
        private void Update()
        {
            this.transform.localPosition = Vector3.zero;
        }
    }
}
