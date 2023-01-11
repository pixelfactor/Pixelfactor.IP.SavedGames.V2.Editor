using Pixelfactor.IP.SavedGames.V2.Editor.EditorObjects;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    [ExecuteInEditMode]
    public class ColourByFaction : MonoBehaviour
    {
        private MeshRenderer meshRenderer = null;

        private MaterialPropertyBlock materialPropertyBlock = null;

        private Color lastAppliedColour;

        public EditorUnit Unit = null;

        void Awake()
        {
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.materialPropertyBlock = new MaterialPropertyBlock();
        }
        private void Update()
        {
            if (this.meshRenderer == null)
                return;

            if (Unit != null)
            {
                var faction = Unit.Faction;

                var color = faction != null ? faction.EditorColor : Color.grey;
                if (color != this.lastAppliedColour)
                {
                    SetColor(color);
                    this.lastAppliedColour = color;
                }
            }
        }

        private void SetColor(Color color)
        {
            if (this.materialPropertyBlock == null)
                this.Awake();

            this.materialPropertyBlock.SetColor("_Color", color);
            this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock);
        }
    }
}
