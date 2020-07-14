using System.Collections.Generic;

[System.Serializable]
public class PokeInfo
{
    public List<Ability> abilities;
    public int base_experience;
    public List<Form> forms;
    public List<GameIndice> game_indices;
    public int height;
    public List<HeldItem> held_items;
    public int id;
    public bool is_default;
    public string location_area_encounters;
    public List<Move> moves;
    public string name;
    public int order;
    public Species species;
    public Sprites sprites;
    public List<Stat> stats;
    public List<Type> types;
    public int weight;
}

[System.Serializable]
public class Ability2
{
    public string name;
    public string url;
}

[System.Serializable]
public class Ability
{
    public Ability2 ability;
    public bool is_hidden;
    public int slot;
}

[System.Serializable]
public class Form
{
    public string name;
    public string url;
}

[System.Serializable]
public class Version
{
    public string name;
    public string url;
}

[System.Serializable]
public class GameIndice
{
    public int game_index;
    public Version version;
}

[System.Serializable]
public class Item
{
    public string name;
    public string url;
}

[System.Serializable]
public class Version2
{
    public string name;
    public string url;
}

[System.Serializable]
public class VersionDetail
{
    public int rarity;
    public Version2 version;
}

[System.Serializable]
public class HeldItem
{
    public Item item;
    public List<VersionDetail> version_details;
}

[System.Serializable]
public class Move2
{
    public string name;
    public string url;
}

[System.Serializable]
public class MoveLearnMethod
{
    public string name;
    public string url;
}

[System.Serializable]
public class VersionGroup
{
    public string name;
    public string url;
}

[System.Serializable]
public class VersionGroupDetail
{
    public int level_learned_at;
    public MoveLearnMethod move_learn_method;
    public VersionGroup version_group;
}

[System.Serializable]
public class Move
{
    public Move2 move;
    public List<VersionGroupDetail> version_group_details;
}

[System.Serializable]
public class Species
{
    public string name;
    public string url;
}

[System.Serializable]
public class Sprites
{
    public string back_default;
    public string back_female;
    public string back_shiny;
    public string back_shiny_female;
    public string front_default;
    public string front_female;
    public string front_shiny;
    public string front_shiny_female;
}

[System.Serializable]
public class Stat2
{
    public string name;
    public string url;
}

[System.Serializable]
public class Stat
{
    public int base_stat;
    public int effort;
    public Stat2 stat;
}

[System.Serializable]
public class Type2
{
    public string name;
    public string url;
}

[System.Serializable]
public class Type
{
    public int slot;
    public Type2 type;
}