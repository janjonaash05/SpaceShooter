using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEmergence : MonoBehaviour
{
    // Start is called before the first frame update


   [SerializeField] Vector3 initScale, targetScale;
    void Start()
    {
        RotateTowardsPlayer();
        Emerge();


        HelperSpawnerManager.OnEMPSpawn += OnEMP ;
    }



    void OnEMP() 
    {
        StopAllCoroutines();
        Shrivel();
    }

    private void OnDestroy()
    {
        HelperSpawnerManager.OnEMPSpawn -= OnEMP;
    }


    void RotateTowardsPlayer() 
    {
        Vector3 rotationDirection = (Camera.main.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(rotationDirection);
        transform.Rotate(Vector3.up, 90);

        transform.localScale = initScale;


        
    }


   public void Emerge()
    {

        


        IEnumerator emerge()
        {

            float lerp = 0f;
            while (transform.localScale.z < targetScale.z)
            {

                lerp += Time.deltaTime / 100;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerp);
                yield return null;

            }



        }

        StartCoroutine(emerge());
    }



    public void Shrivel()
    {
        IEnumerator shrivel()
        {

            float lerp = 0f;
            targetScale = Vector3.zero;
            while (transform.localScale.z > targetScale.z)
            {

                lerp += Time.deltaTime / 100;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerp);
                yield return null;

            }

            Destroy(gameObject);

        }

        StartCoroutine(shrivel());
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
