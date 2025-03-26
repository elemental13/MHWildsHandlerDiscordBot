using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace WildsApi {
    public class Monster {
        public int id { get; set;}
        public MonsterKind kind { get; set;}
        public string? name { get; set;}
        public string? description { get; set;}
        public List<Location>? locations { get; set; }
        public List<Ailment>? ailments { get; set; }
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
        [Display(Name="bird wyvern")]
        BirdWyvern,
        [Display(Name="brute wyvern")]
        BruteWyvern,
        [Display(Name="elder dragon")]
        ElderDragon,
        [Display(Name="fanged wyvern")]
        FangedWyvern,
        [Display(Name="fish")]
        Fish,
        [Display(Name="flying wyvern")]
        FlyingWyvern,
        [Display(Name="herbivore")]
        Herbivore,
        [Display(Name="lynian")]
        Lynian,
        [Display(Name="neopteron")]
        Neopteron,
        [Display(Name="piscine wyvern")]
        PiscineWyvern,
        [Display(Name="relict")]
        Relict,
        [Display(Name="wingdrake")]
        Wingdrake,
        [Display(Name="fanged beast")]
        FangedBeast,
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
        public List<SkillRank>? ranks { get; set; }
        public SkillKind kind { get; set; }
    }

    public class SkillRank {
        public int id { get; set;}
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
        public ResistanceKind kind { get; set; }
        public Element element { get; set; }
        public Status status { get; set; }
    }

    public enum ResistanceKind {
        element,
        status,
    }

    public class MonsterWeaknesses {
        public int id { get; set;}
        public string? condition { get; set; }
        public WeaknessKind kind { get; set; }
        public Element element { get; set; } // depends on kind, a tagged union type, could be null
        public Status status { get; set;} // depends on kind, a tagged union type, could be null
    }

    public enum WeaknessKind {
        element,
        status,
    }

    public class MonsterReward {
        public Item? item { get; set; }
        public List<RewardCondition>? conditions { get; set; }
    }

    public class RewardCondition {
        public RewardConditionKind kind { get; set; }
        public Rank rank { get; set; }
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
    }
}