using System;
using System.Collections.Generic;

namespace SimpleSaveSystem.Example.SaveExample
{
    [Serializable]
    public class MainSavegame
    {
        public string Name;
        public List<ItemSave> Items;
        public ItemContainerSave ItemContainer;
    }
}