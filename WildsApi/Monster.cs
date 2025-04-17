using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WildsApi {
    // potential webscrape site for monster data is https://mhwilds.kiranico.com/data/monsters but id rather get it from the docs.mhwilds database with query language
    public class Monster {
        public int id { get; set;}
        [JsonConverter(typeof(JsonStringEnumConverter<MonsterKind>))]
        public MonsterKind kind { get; set;}
        [JsonConverter(typeof(JsonStringEnumConverter<Species>))]
        public Species species { get; set;}
        public string? name { get; set;}
        public string? description { get; set;}
        public List<Location>? locations { get; set; }
        public List<Ailment>? ailments { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Element>))]
        public List<Element>? elements { get; set; }
        public List<MonsterResistance>? resistances { get; set; }
        public List<MonsterWeaknesses>? weaknesses { get; set; }
        public List<MonsterReward>? rewards { get; set; }
    }

    public enum MonsterKind {
        small,
        large,
    }

    public enum Species {
        [EnumMember(Value = "bird-wyvern")]
        BirdWyvern,
        [EnumMember(Value = "brute-wyvern")]
        BruteWyvern,
        [EnumMember(Value = "elder-dragon")]
        ElderDragon,
        [EnumMember(Value = "fanged-wyvern")]
        FangedWyvern,
        [EnumMember(Value = "fish")]
        Fish,
        [EnumMember(Value = "flying-wyvern")]
        FlyingWyvern,
        [EnumMember(Value = "herbivore")]
        Herbivore,
        [EnumMember(Value = "lynian")]
        Lynian,
        [EnumMember(Value = "neopteron")]
        Neopteron,
        [EnumMember(Value = "piscine-wyvern")]
        PiscineWyvern,
        [EnumMember(Value = "relict")]
        Relict,
        [EnumMember(Value = "wingdrake")]
        Wingdrake,
        [EnumMember(Value = "fanged-beast")]
        FangedBeast,
        [EnumMember(Value = "construct")]
        Construct,
        [EnumMember(Value = "temnoceran")]
        Temnoceran,
        [EnumMember(Value = "demi-elder")]
        DemiElder,
        [EnumMember(Value = "cephalopod")]
        Cephalopod,
        [EnumMember(Value = "carapaceon")]
        Carapaceon,
        [EnumMember(Value = "leviathan")]
        Leviathan,
        [EnumMember(Value = "amphibian")]
        Amphibian,
        [EnumMember(Value = "snake-wyvern")]
        SnakeWyvern,
    }

    public class Location {
        public int id { get; set; }
        public string? name { get; set; }
        public int zoneCount { get; set; }
        public List<Camp>? camps { get; set; }
    }

    public class Camp {
        public int id { get; set;}
        public string? name { get; set; }
        public int zone { get; set; }
    }

    public class Ailment {
        public int id { get; set;}
        public string? name { get; set; }
        public Recovery? recovery { get; set; }
        public Protection? protection { get; set; }
    }

    public class Recovery {
        public List<string>? actions { get; set; }
        public List<Item>? items { get; set; }
    }

    public class Protection {
        public List<Item>? items { get; set; }
        public List<Skill>? skills { get; set; }
    }

    public class Item {
        public int id { get; set;}
        public int gameId { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public int rarity { get; set; }
        public int carryLimit { get; set; }
        public int value { get; set; }
        public List<ItemRecipe>? recipes { get; set; }
    }

    public class ItemRecipe {
        public int id { get; set;}
        public int amount { get; set; }
        public List<Item>? inputs { get; set; }
    }

    public class Skill {
        public int id { get; set;}
        public string? name { get; set; }
        public string? description { get; set; }
        public int level { get; set; }
        public List<SkillRank>? ranks { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<SkillKind>))]
        public SkillKind kind { get; set; }
    }

    public class SkillRank {
        public int id { get; set;}
        public string? name { get; set; }
        public string? description { get; set; }
        public int level { get; set; }
    }

    public enum SkillKind {
        armor,
        weapon,
        set,
        group,
    }

    public enum Rank {
        low,
        high,
        master,
    }

    public enum Element {
        fire,
        water,
        ice,
        thunder,
        dragon,
        blast,
    }

    public enum Status {
        poison,
        sleep,
        paralysis,
        stun,
        blastblight,
    }

    public enum DamageKind {
        severing,
        blunt,
        projectile,
    }

    public enum Elderseal {
        low,
        average,
        high,
    }

    public class MonsterResistance {
        public int id { get; set;}
        public string? condition { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<ResistanceKind>))]
        public ResistanceKind? kind { get; set; }
        public Element? element { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Status>))]
        public Status? status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Effect>))]
        public Effect? effect { get; set; }
    }

    public enum ResistanceKind {
        element,
        status,
        effect,
    }

    public class MonsterWeaknesses {
        public int id { get; set;}
        public string? condition { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<WeaknessKind>))]
        public WeaknessKind kind { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Element>))]
        public Element? element { get; set; } // depends on kind, a tagged union type, could be null
        [JsonConverter(typeof(JsonStringEnumConverter<Status>))]
        public Status? status { get; set;} // depends on kind, a tagged union type, could be null
        [JsonConverter(typeof(JsonStringEnumConverter<Effect>))]
        public Effect? effect { get; set; } // depends on kind, a tagged union type, could be null
    }

    public enum WeaknessKind {
        element,
        status,
        effect,
    }

    public class MonsterReward {
        public Item? item { get; set; }
        public List<RewardCondition>? conditions { get; set; }
    }

    public class RewardCondition {
        [JsonConverter(typeof(JsonStringEnumConverter<RewardConditionKind>))]
        public RewardConditionKind? kind { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<Rank>))]
        public Rank? rank { get; set; }
        public int quantity { get; set; }
        public int chance { get; set; }
        public string? subtype { get; set; }
    }

    public enum RewardConditionKind {
        carve,
        investigation,
        mining,
        palico,
        reward,
        shiny,
        track,
        wound,
        [EnumMember(Value = "target-reward")]
        targetreward,
        [EnumMember(Value = "carve-rotten")]
        carverotten,
        [EnumMember(Value = "wound-destroyed")]
        wounddestroyed,
        [EnumMember(Value = "carve-rotten-severed")]
        carverottensevered,
        [EnumMember(Value = "wound-destroyed-severed")]
        wounddestroyedsevered,
        [EnumMember(Value = "broken-part")]
        brokenpart,
        [EnumMember(Value = "carve-severed")]
        carvesevered,
        [EnumMember(Value = "carve-crystallized")]
        carvecrystallized,
        [EnumMember(Value = "tempered-wound-destroyed")]
        temperedwounddestroyed,
        tempered,
    }

    public enum Effect {
        noise,
        flash,
        stun,
        exhaust,
    }
}