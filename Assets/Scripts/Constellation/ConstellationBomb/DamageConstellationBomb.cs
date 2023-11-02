using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DamageConstellationBomb : DamageBomb
{
    // Start is called before the first frame update
    public override async Task StartDamage(bool _) //empty bool for playerTargeted in base class
    {

        Destroy(GetComponent<Collider>());


        Destroy(GetComponent<ConstellationBombFall>());

        await GetComponent<ConstellationBombColorChange>().CoverInColor();


         ScaleDown(token);

       
    } 
}
