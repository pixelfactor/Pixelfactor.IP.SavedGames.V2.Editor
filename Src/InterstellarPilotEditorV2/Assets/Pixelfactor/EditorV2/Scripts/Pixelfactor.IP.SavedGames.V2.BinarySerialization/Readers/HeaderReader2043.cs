using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using System.Globalization;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers
{
    public class HeaderReader2043 : ISavedGameHeaderReader
    {
        public ISavedGameHeader Read(BinaryReader reader)
        {
            var header = new ModelHeader();

            header.SaveVersion = reader.ReadVersion();
            header.CreatedVersion = reader.ReadVersion();
            header.IsAutoSave = reader.ReadBoolean();
            header.TimeStamp = DateTime.ParseExact(reader.ReadString(), Constants.HeaderDateFormat, new CultureInfo("en-GB"));
            header.ScenarioInfoId = reader.ReadInt32();
            header.GlobalSaveNumber = reader.ReadInt32();
            header.SaveNumber = reader.ReadInt32();
            header.HavePlayer = reader.ReadBoolean();
            header.PlayerSectorName = reader.ReadString();
            header.PlayerName = reader.ReadString();
            header.Credits = reader.ReadInt64();
            header.FactionName = reader.ReadString();
            header.NetWorth = reader.ReadInt64();
            header.Permadeath = reader.ReadBoolean();
            header.SecondsElapsed = reader.ReadDouble();
            header.GameStartDate = DateTime.ParseExact(reader.ReadString(), Constants.HeaderDateFormat, new CultureInfo("en-GB"));
            header.ScenarioTitle = reader.ReadString();
            header.ScenarioAuthor = reader.ReadString();
            header.ScenarioAuthoringTool = reader.ReadString();
            header.ScenarioDescription = reader.ReadString();

            return header;
        }

        public static Version ReadVersionOnly(BinaryReader reader)
        {
            return(reader.ReadVersion());
        }
    }
}
