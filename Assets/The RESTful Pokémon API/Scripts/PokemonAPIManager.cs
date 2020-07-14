using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class PokemonAPIManager : MonoBehaviour
{
    private readonly string basePokeURL = "https://pokeapi.co/api/v2/";

    [Header("Setup")]
    public RawImage pokemonImagePlaceholder1;

    public TextMeshProUGUI pokeNameTMP, pokeIdTMP, pokeTypesTMP;

    public TMP_InputField searchFieldTMP;

    [Header("Poke Info")]
    public PokeInfo pokeInfo;


    void Start()
    {
        pokeNameTMP.text = "";
        pokeIdTMP.text = "";
        pokeTypesTMP.text = "";
    }


    public void OnButtonRandomPokemon()
    {
        int randomPokeIndex = Random.Range(1, 808); // Min: inclusive, Max: exclusive

      

        StartCoroutine(GetPokemonAtIndex(randomPokeIndex.ToString()));
    }

    public void OnButtonSearch()
    {
        if(!string.IsNullOrWhiteSpace(searchFieldTMP.text))
        StartCoroutine(GetPokemonAtIndex(searchFieldTMP.text));
    }

    private IEnumerator GetPokemonAtIndex(string pokemonIndex)
    {
        pokeNameTMP.text = "Loading. . .";
        pokeIdTMP.text = "#. . .";
        pokeTypesTMP.text = ". . .";

        searchFieldTMP.text = pokemonIndex;
        // Get Pokemon Info
        // Example URL: https://pokeapi.co/api/v2/pokemon/151

        string pokemonURL = basePokeURL + "pokemon/" + pokemonIndex;

       
        var www = new UnityWebRequest(pokemonURL)
        {
            // or use var www = UnityWebRequest.Get(pokemonURL); will work 2
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {

            pokeNameTMP.text = "Error";
            searchFieldTMP.text = "";
            Debug.Log("Error has occur: " + www.error);
            yield break;
        }

        pokeInfo = JsonUtility.FromJson<PokeInfo>(www.downloadHandler.text);
        PokemonDetail(pokeInfo);

        pokeNameTMP.text = char.ToUpper(pokeInfo.name[0]) + pokeInfo.name.Substring(1);
       
        pokeIdTMP.text = "#" + pokeInfo.id;
        pokeTypesTMP.text = "";
        foreach (var type in pokeInfo.types)
        {
            pokeTypesTMP.text += type.type.name +"\n";
        }

        UnityWebRequest pokeSpriteRequest1 = UnityWebRequestTexture.GetTexture(pokeInfo.sprites.front_default);

        var asyncOperation = pokeSpriteRequest1.SendWebRequest();

        if (!pokeSpriteRequest1.isNetworkError || !pokeSpriteRequest1.isHttpError)
        {
            while (!pokeSpriteRequest1.isDone)
            {
                Debug.Log(asyncOperation.progress);
                yield return null;
            }
        }
        else
        {
            Debug.Log("Error has occur: " + pokeSpriteRequest1.error);
            yield break;
        }


        // If Sprite Image, use Sprite.Create(DownloadHandlerTexture.GetContent(pokeSpriteRequest1);
        pokemonImagePlaceholder1.texture = DownloadHandlerTexture.GetContent(pokeSpriteRequest1);
        pokemonImagePlaceholder1.texture.filterMode = FilterMode.Point;
    }

    public void PokemonDetail(PokeInfo pokeInfo)
    {
        // Name
        Debug.Log($"Poke Name: {pokeInfo.name}");
        // Ability
        foreach (var ability in pokeInfo.abilities)
        {
            Debug.Log($"<color=#ED4C67>Ability Name: {ability.ability.name}, IsHidden: {ability.is_hidden}, Slot: {ability.slot}</color>\n{ability.ability.url}");
        }

        Debug.Log($"<color=#FFC312>Base Experience: {pokeInfo.base_experience}</color>");

        // Form
        foreach (var form in pokeInfo.forms)
        {
            Debug.Log($"<color=#006266>Form: {form.name}</color>\n{form.url}");
        }

        // Game Indice
        foreach(var gameIndice in pokeInfo.game_indices)
        {
            Debug.Log($"<color=#D980FA>Game index: {gameIndice.game_index}, Version Name: {gameIndice.version.name}</color>\n{gameIndice.version.url}");
        }

        // Height
        Debug.Log($"<color=#A3CB38>Height: {pokeInfo.height}</color>");

        // HeldI Item
        foreach (var heldItem in pokeInfo.held_items)
        {
            Debug.Log($"<color=#1B1464>Item name: {heldItem.item.name}</color>\n{heldItem.item.url}");
            foreach (var versionDetail in heldItem.version_details)
            {
                Debug.Log($"<color=#006266>Version Detail Rarity: {versionDetail.rarity}, Version Name: {versionDetail.version.name}</color>\n{versionDetail.version.url}");
            }
        }

        // ID
        Debug.Log($"Poke ID: #{pokeInfo.id}");

        // Is Default
        Debug.Log($"Is Default: {pokeInfo.is_default}");

        // Location Area Encounters
        Debug.Log($"Location Area Encounters: {pokeInfo.location_area_encounters}");

        // Move
        foreach (var move in pokeInfo.moves)
        {
            Debug.Log($"<color=blue>Move {move.move.name}</color>\n{move.move.url}");
            foreach (var versionGroupDetail in move.version_group_details)
            {
                Debug.Log($"Version Group Detail Level learned at: {versionGroupDetail.level_learned_at}");
                Debug.Log($"Move Learn Method Name: {versionGroupDetail.move_learn_method.name}\n{versionGroupDetail.move_learn_method.url}");
                Debug.Log($"Version Group: {versionGroupDetail.version_group.name}\n{versionGroupDetail.version_group.url}");
            }
        }

        // Order
        Debug.Log($"Order: {pokeInfo.order}");

        // Species
        Debug.Log($"Species: {pokeInfo.species.name}\n{pokeInfo.species.url}");

        // Sprites
        Debug.Log($"back_default: {pokeInfo.sprites.back_default}");
        Debug.Log($"back_female: {pokeInfo.sprites.back_female}");
        Debug.Log($"back_shiny: {pokeInfo.sprites.back_shiny}");
        Debug.Log($"back_shiny_female: {pokeInfo.sprites.back_shiny_female}");
        Debug.Log($"front_default: {pokeInfo.sprites.front_default}");
        Debug.Log($"front_shiny: {pokeInfo.sprites.front_shiny}");
        Debug.Log($"front_shiny_female: {pokeInfo.sprites.front_shiny_female}");

        // Stats
        foreach (var stat in pokeInfo.stats)
        {
            Debug.Log($"Stat: {stat.stat.name}, Base stat: {stat.base_stat}, Effort: {stat.effort}\n{stat.stat.url}");
        }

        // 
        foreach (var type in pokeInfo.types)
        {
            Debug.Log($"Slot: {type.slot}, Type Name: {type.type.name}\n{type.type.url}");
        }
    }
}
