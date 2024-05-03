using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenTransportColorChange : MonoBehaviour
{
    


    Material default_color;




    bool perma_stopped = false;



    void OnLaserShotPlayerDeath() => perma_stopped = true;


    void Start()
    {
        rend = GetComponent<Renderer>();
        default_color = rend.materials[2];



        SpinnerChargeUp.OnLaserShotPlayerDeath += OnLaserShotPlayerDeath;


    }



    private void OnDestroy()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath -= OnLaserShotPlayerDeath;
    }


    
  
    Renderer rend;




    /// <summary>
    /// For a set number of iterations:
    /// <para>- Breaks and returns if perma_stopped is true.</para>
    /// <para>- If the current iteration is even, plays the TOKEN_SPAWN sound. </para>
    /// <para>- Sets the renderer material at a specific index to either the arg mat or the default color, based on if the current iteration is even. </para>
    /// </summary>
    /// <param name="material"></param>
    /// <returns></returns>
    public IEnumerator Flash(Material material)
    {

        for (int i = 0; i < 8; i++)
        {
            if (perma_stopped) yield break;

            if (i % 2 == 0) { AudioManager.PlayActivitySound(AudioManager.ActivityType.TOKEN_SPAWN); }

            rend.materials = new Material[] { rend.materials[0], rend.materials[1], i % 2 == 0 ?  material : default_color, };
            yield return new WaitForSeconds(0.5f);


        }



    }




    /// <summary>
    /// At a specific index, sets the material to arg material, waits arg delay, sets the material back to SIDE_TOOLS_COLOR.
    /// </summary>
    /// <param name="material"></param>
    /// <param name="delay"></param>
    public void SetColorDelayed(Material material, float delay) 
    {

        IEnumerator set()
        {
            rend.materials = new Material[] { rend.materials[0], rend.materials[1], material };
            yield return new WaitForSeconds(delay);

            rend.materials = new Material[] { rend.materials[0], rend.materials[1], MaterialHolder.Instance().SIDE_TOOLS_COLOR() };

        }

        StartCoroutine(set());

       

    }











}
