using System.Runtime.Serialization;

namespace WildsApi {
    public class Armor {
        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public ArmorKind kind { get; set; }
        public Rank rank { get; set; }
        public int rarity { get; set; }
        public ArmorDefense? defense { get; set; }
        public ArmorResistances? resistances { get; set; }
        public List<int>? slots { get; set; }
        public List<SkillRank>? skills { get; set; }
        public ArmorSet? armorSet { get; set; }
        public ArmorCrafting? armorCrafting { get; set; }
    }

    public enum ArmorKind {
        [EnumMember(Value = "head")]
        head,
        [EnumMember(Value = "chest")]
        chest,
        [EnumMember(Value = "arms")]
        arms,
        [EnumMember(Value = "waist")]
        waist,
        [EnumMember(Value = "legs")]
        legs
    }

    public class ArmorDefense {
        public int @base { get; set; }
        public int max { get; set; }
    }

    public class ArmorResistances {
        public int fire { get; set; }
        public int water { get; set; }
        public int ice { get; set; }
        public int thudner { get; set; }
        public int dragon { get; set; }
    }

    public class ArmorSet {
        public int id { get; set; }
        public int gameId { get; set; }
        public string? name { get; set; }
        public List<Armor>? pieces { get; set; }
        public ArmorSetBonus? bonus { get; set; }
        public ArmorSetBonus? groupBonus { get; set; }
    }

    public class ArmorSetBonus {
        public int id { get; set;}
        public Skill? skill { get; set; }
        public List<ArmorSetBonusRank>? ranks { get; set; }
    }

    public class ArmorSetBonusRank {
        public int id { get; set;}
        public int pieces { get; set; }
        public SkillRank? skill { get; set; }
    }

    public class ArmorCrafting {
        public int id { get; set;}
        public int zennyCost { get; set; }
        public List<CraftingCost>? materials { get; set; }
    }

    public class CraftingCost {
        public int quantity { get; set; }
        public Item? item { get; set; }
    }
}