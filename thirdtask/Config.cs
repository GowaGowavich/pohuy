using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace thirdtask
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Шанс получение урона при обработке в 914")]
        public int ChanceDamage { get; set; } = 20;

        [Description("Шанс получение урона при обработке в 914")]
        public int ChanceEffect { get; set; } = 20;

        [Description("Шанс телепорта в случайную комнату при обработке в 914")]
        public int ChanceTp { get; set; } = 20;

        [Description("Список негативных эффектов")]
        public List<EffectType> NegEffect { get; set; } = new List<EffectType> { };
    }
}
