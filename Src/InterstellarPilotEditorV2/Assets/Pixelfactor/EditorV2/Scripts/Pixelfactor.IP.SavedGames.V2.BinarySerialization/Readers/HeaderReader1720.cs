using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using System.Globalization;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers
{
    public class HeaderReader1720 : ISavedGameHeaderReader
    {
        public ISavedGameHeader Read(BinaryReader reader)
        {
            var header = new ModelHeader();

            header.Version = reader.ReadVersion();
            header.IsAutoSave = reader.ReadBoolean();
            header.TimeStamp = DateTime.ParseExact(reader.ReadString(), Constants.HeaderDateFormat, new CultureInfo("en-GB"));
            header.ScenarioInfoId = reader.ReadInt32();
            header.GlobalSaveNumber = reader.ReadInt32();
            header.SaveNumber = reader.ReadInt32();
            header.HavePlayer = reader.ReadBoolean();
            if (header.HavePlayer)
            {
                header.PlayerSectorName = reader.ReadString();
                header.PlayerName = reader.ReadString();
                header.Credits = reader.ReadInt32();
            }

            return header;
        }

        public static Version ReadVersionOnly(BinaryReader reader)
        {
            return (reader.ReadVersion());
        }
    }
}
