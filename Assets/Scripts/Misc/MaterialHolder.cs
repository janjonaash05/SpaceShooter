using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] Material[] _COLOR_SET,
        _TURRET_CONTROL_1_COLOR_SET, _TURRET_CONTROL_2_COLOR_SET, _PLAYER_HEALTH_SET;




    [SerializeField] Material _TURRET_CONTROL_AUTO_COLOR_ON, _TURRET_CONTROL_AUTO_COLOR_OFF, _SIDE_TOOLS_COLOR;


    [SerializeField] Material _FRIENDLY_PRIMARY;
    [SerializeField] Material _FRIENDLY_SECONDARY;

    [SerializeField] Material _ENEMY_PRIMARY;

    [SerializeField] Material _ENEMY_SECONDARY;


    [SerializeField] Material _PLAYER_HEALTH;


    public Material[] COLOR_SET_WHOLE() =>  _COLOR_SET;


    public Material[] COLOR_SET_1() => _TURRET_CONTROL_1_COLOR_SET;

    public Material[] COLOR_SET_2() => _TURRET_CONTROL_2_COLOR_SET;


    public Material[] PLAYER_HEALTH_SET() => _PLAYER_HEALTH_SET;




    public Material TURRET_CONTROL_1_UP_RIGHT() => _TURRET_CONTROL_1_COLOR_SET[1];
    public Material TURRET_CONTROL_1_UP_LEFT() => _TURRET_CONTROL_1_COLOR_SET[3];
    public Material TURRET_CONTROL_1_DOWN_RIGHT() => _TURRET_CONTROL_1_COLOR_SET[2];
    public Material TURRET_CONTROL_1_DOWN_LEFT() => _TURRET_CONTROL_1_COLOR_SET[0];




    public Material TURRET_CONTROL_2_UP_RIGHT() => _TURRET_CONTROL_2_COLOR_SET[3];
    public Material TURRET_CONTROL_2_UP_LEFT() => _TURRET_CONTROL_2_COLOR_SET[1];
    public Material TURRET_CONTROL_2_DOWN_RIGHT() => _TURRET_CONTROL_2_COLOR_SET[0];
    public Material TURRET_CONTROL_2_DOWN_LEFT() => _TURRET_CONTROL_2_COLOR_SET[2];




    public Material SIDE_TOOLS_COLOR() => _SIDE_TOOLS_COLOR;
    public Material TURRET_CONTROL_AUTO_COLOR_ON() => _TURRET_CONTROL_AUTO_COLOR_ON;
    public Material TURRET_CONTROL_AUTO_COLOR_OFF() => _TURRET_CONTROL_AUTO_COLOR_OFF;

    public Material FRIENDLY_PRIMARY() => _FRIENDLY_PRIMARY;
    public Material FRIENDLY_SECONDARY() => _FRIENDLY_SECONDARY;


    public Material ENEMY_PRIMARY() => _ENEMY_PRIMARY;
    public Material ENEMY_SECONDARY() => _ENEMY_SECONDARY;



    



    public static MaterialHolder Instance()
    {
        return GameObject.FindWithTag(Tags.MATERIAL_HOLDER).GetComponent<MaterialHolder>();
    
    
    }
}
