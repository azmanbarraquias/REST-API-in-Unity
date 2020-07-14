using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardSet
{
    public string set_name;
    public string set_code;
    public string set_rarity;
    public string set_price;
}

[System.Serializable]
public class CardImage
{
    public int id;
    public string image_url;
    public string image_url_small;
}

[System.Serializable]
public class CardPrice
{
    public string cardmarket_price;
    public string tcgplayer_price;
    public string coolstuffinc_price;
    public string ebay_price;
    public string amazon_price;
}

[System.Serializable]
public class BanlistInfo
{
    public string ban_tcg;
    public string ban_ocg;
    public string ban_goat;
}

[System.Serializable]
public class CardInfo
{
    public int id;
    public string name;
    public string type;
    public string desc;
    public string race;
    public string archetype;
    public List<CardSet> card_sets;
    public List<CardImage> card_images;
    public List<CardPrice> card_prices;
    public int atk;
    public int def;
    public int level;
    public string attribute;
    public BanlistInfo banlist_info;
    public int scale;
    public int linkval;
    public List<string> linkmarkers;
}

[System.Serializable]
public class Cards
{
    public List<CardInfo> cards;
}