using System.Collections.Generic;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public class Styles
    {
        public static GUILayoutOption[] BigButton
        {
            get
            {
                return new GUILayoutOption[]
                {
                    GUILayout.Width(200),
                    GUILayout.Height(50)
                };
            }
        }

        public static GUILayoutOption[] Button
        {
            get
            {
                return new GUILayoutOption[]
                {
                    GUILayout.Width(180),
                    GUILayout.Height(30)
                };
            }
        }
    }
}
