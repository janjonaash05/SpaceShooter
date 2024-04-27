using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;



[Serializable]
public class NameMaterialPair
{
    [SerializeField] COLOR colorName;
    [SerializeField] Material material;

    public NameMaterialPair(COLOR colorName, Material material)
    {
        this.colorName = colorName;
        this.material = material;
    }







    public COLOR ColorName => colorName;
    public Material Material => material;
}

public class MaterialHolder : MonoBehaviour
{



    



    [SerializeField]
    Material[] _COLOR_SET,
        _COLOR_SET_1, _COLOR_SET_2, _PLAYER_HEALTH_SET;




    [SerializeField] Material _TURRET_CONTROL_AUTO_COLOR_ON, _TURRET_CONTROL_AUTO_COLOR_OFF, _SIDE_TOOLS_COLOR;


    [SerializeField] Material _FRIENDLY_PRIMARY;
    [SerializeField] Material _FRIENDLY_SECONDARY;

    [SerializeField] Material _ENEMY_PRIMARY;

    [SerializeField] Material _ENEMY_SECONDARY;


    [SerializeField] Material _ENEMY_UPGRADE;
    [SerializeField] Material _FRIENDLY_UPGRADE;


    [SerializeField] Material _PLAYER_HEALTH;

    [SerializeField] float darkening_intensity;

    public Material[] COLOR_SET_WHOLE() => _COLOR_SET;


    public Material[] COLOR_SET_1() => _COLOR_SET_1;

    public Material[] COLOR_SET_2() => _COLOR_SET_2;


    NameMaterialPair[] _NAMED_COLOR_SET_1, _NAMED_COLOR_SET_2;


    public NameMaterialPair[] NAMED_COLOR_SET_1() => _NAMED_COLOR_SET_1;
    public NameMaterialPair[] NAMED_COLOR_SET_2() => _NAMED_COLOR_SET_2;



    public Material[] PLAYER_HEALTH_SET() => _PLAYER_HEALTH_SET;




    [SerializeField] NameMaterialPair[] _NAMED_MATERIAL_PAIRS;
    public NameMaterialPair[] NAMED_COLOR_SET_WHOLE() => _NAMED_MATERIAL_PAIRS;


    public Dictionary<COLOR, Material> NAME_MATERIAL_DICT { get; private set; } = new();




    public static void Awake() => Instance().Init();


    public void Init()
    {
        foreach (var item in _NAMED_MATERIAL_PAIRS)
        {

            Debug.Log("Adding "+item.ColorName);
            NAME_MATERIAL_DICT.Add(item.ColorName, item.Material);

        }

        _NAMED_COLOR_SET_1 = new NameMaterialPair[] 
        { 
            new(COLOR.BLUE, NAME_MATERIAL_DICT[COLOR.BLUE]), 
            new(COLOR.GREEN, NAME_MATERIAL_DICT[COLOR.GREEN]), 
            new(COLOR.YELLOW, NAME_MATERIAL_DICT[COLOR.YELLOW]), 
            new(COLOR.RED, NAME_MATERIAL_DICT[COLOR.RED]) 
        };





        _NAMED_COLOR_SET_2 = new NameMaterialPair[]
        {
            new(COLOR.CYAN, NAME_MATERIAL_DICT[COLOR.CYAN]),
            new(COLOR.TEAL, NAME_MATERIAL_DICT[COLOR.TEAL]),
            new(COLOR.MAGENTA, NAME_MATERIAL_DICT[COLOR.MAGENTA]),
            new(COLOR.ORANGE, NAME_MATERIAL_DICT[COLOR.ORANGE])
        };





    }



    public Material SIDE_TOOLS_COLOR() => _SIDE_TOOLS_COLOR;
    public Material TURRET_CONTROL_AUTO_COLOR_ON() => _TURRET_CONTROL_AUTO_COLOR_ON;
    public Material TURRET_CONTROL_AUTO_COLOR_OFF() => _TURRET_CONTROL_AUTO_COLOR_OFF;

    public Material FRIENDLY_PRIMARY() => _FRIENDLY_PRIMARY;
    public Material FRIENDLY_SECONDARY() => _FRIENDLY_SECONDARY;


    public Material ENEMY_PRIMARY() => _ENEMY_PRIMARY;
    public Material ENEMY_SECONDARY() => _ENEMY_SECONDARY;

    public Material ENEMY_UPGRADE() => _ENEMY_UPGRADE;

    public Material FRIENDLY_UPGRADE() => _FRIENDLY_UPGRADE;






    public static MaterialHolder Instance()
    {
        return GameObject.FindWithTag(Tags.MATERIAL_HOLDER).GetComponent<MaterialHolder>();


    }
}
