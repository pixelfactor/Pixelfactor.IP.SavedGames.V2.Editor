using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Import
{
    public class ImportOperation
    {
        private EditorSavedGame editorSavedGame;
        private SavedGame savedGameModel;
        private Transform sectorsTransform = null;

        public ImportOperation(SavedGame savedGameModel, EditorSavedGame editorSavedGame)
        {
            this.editorSavedGame = editorSavedGame;
            this.savedGameModel = savedGameModel;

            this.sectorsTransform = this.editorSavedGame.SectorsRoot != null ? this.editorSavedGame.SectorsRoot : this.editorSavedGame.transform;
        }

        public void Import()
        {
            this.ImportSectors();
        }

        private void ImportSectors()
        {
            
        }
    }
}
