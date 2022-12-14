using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Writers
{
    public class HeaderWriter
    {
        public void Write(BinaryWriter writer, ModelHeader header)
        {
            writer.WriteVersion(header.Version ?? new Version(1, 0, 0));
            writer.WriteVersion(header.CreatedVersion ?? new Version(1, 0, 0));
            writer.Write(header.IsAutoSave);
            writer.Write(DateTime.Now.ToString(Constants.HeaderDateFormat));
            writer.Write(header.ScenarioInfoId);
            writer.Write(header.GlobalSaveNumber);
            writer.Write(header.SaveNumber);
            writer.Write(true); // Previously this was a 'Have Player' value. No longer set but wrote for backwards compatibility
            writer.WriteStringOrEmpty(header.PlayerSectorName);
            writer.WriteStringOrEmpty(header.PlayerName);
            writer.Write(header.Credits);
            writer.WriteStringOrEmpty(header.FactionName);
            writer.Write(header.NetWorth);
            writer.Write(header.Permadeath);
            writer.Write(header.SecondsElapsed);
            writer.Write(header.GameStartDate.ToString(Constants.HeaderDateFormat));
            writer.WriteStringOrEmpty(header.ScenarioTitle);
        }
    }
}
