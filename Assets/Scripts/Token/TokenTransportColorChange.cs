using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenTransportColorChange : MonoBehaviour
{
    // Start is called before the first frame update

    const int SPAWN_WAIT_TIME = 4;

    Material default_color;

    void Start()
    {
        rend = GetComponent<Renderer>();
        default_color = rend.materials[2];
    }

    // Update is called once per frame
    void Update()
    {

    }

    Renderer rend;



    public IEnumerator Flash(Material material)
    {

        for (int i = 0; i < 8; i++)
        {

            rend.materials = new Material[] { rend.materials[0], rend.materials[1], i % 2 == 0 ?  material : default_color, };
            yield return new WaitForSeconds(0.5f);


        }



    }



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
