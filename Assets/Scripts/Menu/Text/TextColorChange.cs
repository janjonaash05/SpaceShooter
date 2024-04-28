using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TextColorChange : MonoBehaviour
{
    [SerializeField] Material[] mats;

    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();



        Engage();


    }


    /// <summary>
    /// Endlessly cycles through the color materials and assigns them with a delay.
    /// </summary>
    public void Engage()
    {

        IEnumerator cycle()
        {
            int index = Random.Range(0, mats.Length);
            while (true)
            {

                rend.material = mats[index];


                index++;

                yield return new WaitForSeconds(1);

                if (index == mats.Length) index = 0;

            }

        }

        StartCoroutine(cycle());






    }



}
