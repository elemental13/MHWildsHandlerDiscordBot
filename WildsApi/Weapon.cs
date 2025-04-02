using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WildsApi {
    public class Weapon {
        public int id {get; set;}
        public int gameId { get; set; }
        public string? name {get; set;}
        // [JsonConverter(typeof(JsonStringEnumConverter<WeaponKind>))]
        public string? kind {get; set;}
        public int rarity {get; set;}
        public WeaponDamage? damage {get; set;}
        public List<WeaponSpecial>? specials {get; set;}
        public Sharpness? sharpness {get; set;}
        public List<int>? handicraft {get; set;}
        public List<SkillRank>? skills {get; set;}
        [JsonConverter(typeof(JsonStringEnumConverter<Elderseal>))]
        public Elderseal? elderseal {get; set;}
        public int affinity {get; set;}
        // this could be represented by the correct picture, a 3 is a three slot jewel, etc etc
        public List<int>? decorations {get; set;}
        public WeaponCrafting? crafting {get; set;}
    }

    // this can be converted to a percentage bar in html, how we do that i have no idea
    public class Sharpness
    {
        public int red {get; set;}
        public int orange {get; set;}
        public int yellow {get; set;}
        public int green {get; set;}
        public int blue {get; set;}
        public int white {get; set;}
        public int purple {get; set;}
    }

    public class WeaponSpecial
    {
        public int id {get; set;}
        public WeaponDamage? damage {get; set;}
        public bool hidden {get; set;}
        // public SpecialKind? kind {get; set;} cant get the fancy union types to work yet, probably needs a custom deserializer
        public string? kind {get; set;}
    }

    public class SpecialKind {
        public WeaponElement? element {get; set;}
        public WeaponStatus? status {get; set;}
    }

    public class WeaponElement {
        [JsonConverter(typeof(JsonStringEnumConverter<Element>))]
        public Element? element {get; set;}
    }

    public class WeaponStatus {
        [JsonConverter(typeof(JsonStringEnumConverter<Status>))]
        public Status? status {get; set;}
    }

    public enum WeaponKind {
        [EnumMember(Value = "bow")]
        bow,
        [EnumMember(Value = "charge-blade")]
        [Display(Name = "charge-blade")]
        chargeblade,
        [EnumMember(Value = "dual-blades")]
        dualblades,
        [EnumMember(Value = "great-sword")]
        greatsword,
        [EnumMember(Value = "gunlance")]
        gunlance,
        [EnumMember(Value = "hammer")]
        hammer,
        [EnumMember(Value = "heavy-bowgun")]
        heavybowgun,
        [EnumMember(Value = "hunting-horn")]
        huntinghorn,
        [EnumMember(Value = "insect-glaive")]
        insectglaive,
        [EnumMember(Value = "lance")]
        lance,
        [EnumMember(Value = "light-bowgun")]
        lightbowgun,
        [EnumMember(Value = "long-sword")]
        longsword,
        [EnumMember(Value = "switch-axe")]
        switchaxe,
        [EnumMember(Value = "sword-shield")]
        swordshield
    }

    public class WeaponDamage {
        public int raw {get; set;}
        public int display {get; set;}
    }

    public class WeaponCrafting {
        public int id { get; set;}
        public bool craftable {get; set;}
        public Weapon? previous {get; set;}
        public List<Weapon>? branches {get; set;}
        public int craftingZennyCost { get; set; }
        public List<CraftingCost>? craftingMaterials { get; set; }
        public int upgradeZennyCost {get; set; }
        public List<CraftingCost>? upgradeMaterials { get; set; }
    }
}