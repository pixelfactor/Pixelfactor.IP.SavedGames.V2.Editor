using Pixelfactor.IP.Common;
using Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers;
using Pixelfactor.IP.SavedGames.V2.Editor.Settings;
using Pixelfactor.IP.SavedGames.V2.Model;
using System;
using System.IO;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor.Tools.Main.Import
{
    /// <summary>
    /// Converts a binary dat file into in-memory game state
    /// </summary>
    public static class BinaryReadTool
    {
        public static Version MinCompatibleSaveVersion = new Version(1, 7, 21);

        public static Version SaveVersion
        {
            get { return CustomSettings.GetOrCreateSettings().SaveVersion; }
        }
        public static ISavedGame ReadFromFile(string filePath, out Version version)
        {
            Debug.Log(string.Format("Loading gamedata from \"{0}\"", filePath));

            // Start saving the game
            using (var reader = new BinaryReader(File.OpenRead(filePath)))
            {
                // Peak at the save version to determine what one to load
                version = ReadVersion(reader);

                var saveGameReader = GetSaveGameReader(version);

                if (saveGameReader != null)
                {
                    // Reset the position in the stream to zero.
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    var savedGame = saveGameReader.Read(reader);

                    Debug.Log("Loading complete...");

                    return savedGame;
                }

                throw new Exception("Unable to load saved game. No compatible reader found");
            }
        }

        public static Version ReadVersion(BinaryReader reader)
        {
            var versionMajor = reader.ReadInt32();
            var versionMinor = reader.ReadInt32();
            var versionBuild = reader.ReadInt32();
            return new Version(versionMajor, versionMinor, versionBuild);
        }

        public static bool IsSaveVersionCompatible(System.Version version)
        {
            return version >= MinCompatibleSaveVersion && version <= SaveVersion;
        }

        private static ISaveGameReader GetSaveGameReader(System.Version version)
        {
            if (version >= MinCompatibleSaveVersion && version <= SaveVersion)
            {
                if (version < SaveVersion)
                {
                    return GetBackwardsCompatibleReader(version);
                }

                return new SaveGameReader();
            }

            return null;
        }

        private static ISaveGameReader GetBackwardsCompatibleReader(Version version)
        {
            // HACK: Reader hasn't changed structure since 2.0.43, but had to give new version ID just to Drake component bay carnage caused in v2.0.49
            if (version >= new Version(2, 0, 43))
            {
                return new SaveGameReader();
            }

            // NOTE: 2.0.18 and 2.0.19 use the same reader but import differently
            if (version >= new Version(2, 0, 18))
            {
                return new SaveGameReader2019();
            }

            if (version >= new Version(2, 0, 17))
            {
                return new SaveGameReader2017();
            }

            if (version >= new Version(2, 0, 11))
            {
                return new SaveGameReader2016();
            }

            if (version >= new Version(2, 0, 4))
            {
                return new SaveGameReader2004();
            }

            if (version >= new Version(2, 0, 3))
            {
                return new SaveGameReader2003();
            }

            if (version >= new Version(1, 7, 30))
            {
                return new SaveGameReader2000();
            }

            // Amend for new version...
            if (version >= new Version(1, 7, 26))
            {
                return new SaveGameReader1726();
            }

            if (version >= new Version(1, 7, 25))
            {
                return new SaveGameReader1725();
            }

            if (version >= new Version(1, 7, 21))
            {
                return new SaveGameReader1721();
            }

            return null;
        }
    }
}
