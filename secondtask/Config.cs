﻿using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace secondtask
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public List<ItemType> Items { get; set; } = new List<ItemType> { ItemType.KeycardO5 };
    }
}
