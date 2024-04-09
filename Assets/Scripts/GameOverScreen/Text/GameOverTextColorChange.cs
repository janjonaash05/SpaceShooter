using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GameOverTextColorChange : MonoBehaviour
{
    [SerializeField] Material[] mats;



    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();



        Engage();


    }



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







    // Update is called once per frame
    void Update()
    {

    }
}
