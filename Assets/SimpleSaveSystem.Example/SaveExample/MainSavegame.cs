using System;
using System.Collections.Generic;

namespace SaveSystem.Example.SaveExample
{
    [Serializable]
    public class MainSavegame
    {
        public string Name;
        public List<ItemSave> Items;
        public ItemContainerSave ItemContainer;
    }
}