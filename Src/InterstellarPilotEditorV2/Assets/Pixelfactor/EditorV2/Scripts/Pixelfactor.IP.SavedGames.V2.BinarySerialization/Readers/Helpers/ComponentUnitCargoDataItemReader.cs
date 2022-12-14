using Pixelfactor.IP.SavedGames.V2.Model;
using System.IO;

namespace Pixelfactor.IP.SavedGames.V2.BinarySerialization.Readers.Helpers
{
    public static class ComponentUnitCargoDataItemReader
    {
        public static ModelComponentUnitCargoDataItem Read(BinaryReader reader)
        {
            var item = new ModelComponentUnitCargoDataItem();
            item.CargoClass = (ModelCargoClass)reader.ReadInt32();
            item.Quantity = reader.ReadInt32();
            return item;
        }
    }
}
