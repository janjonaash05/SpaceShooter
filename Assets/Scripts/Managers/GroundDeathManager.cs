using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundDeathManager : MonoBehaviour
{










    public static event Action
        OnCoreDeath,
        OnCoreRingDeath,
        OnGroundMainPrimaryDeath,
        OnGroundMainSecondaryDeath,
        OnControlPadsDeath,
        OnGroundSideSecondaryDeath,
        OnGroundSidePrimaryDeath,

        OnControlStandsDeath, OnSliderRechargeStationDeath, OnTurretPillarsDeath,
        OnControlHeadsDeath, OnHarpoonControlHeadsDeath, OnSliderRechargeHeadsDeath, OnTurretStationsDeath, OnHarpoonHeadDeath, OnSliderHeadDeath,
        OnTurretHeadsDeath,
        OnShieldAdapterDeath, OnShieldStorageDeath, OnShieldEmitterDeath, OnTokenTransporterDeath;



    const float WAIT_TIME = 0.2f;


    static readonly Dictionary<int, Action>  execution_order_dict = new()
    {
        {1, () => OnCoreDeath?.Invoke() },
        {2, () => OnCoreRingDeath?.Invoke() },
        {3, () => OnGroundMainPrimaryDeath?.Invoke() },
        {4, () => OnGroundMainSecondaryDeath?.Invoke() },
        {5, () => OnControlPadsDeath?.Invoke() },
        {6, () => OnGroundSideSecondaryDeath?.Invoke() },
        {7, () => OnGroundSidePrimaryDeath?.Invoke() },
        {8, () =>{ OnControlStandsDeath?.Invoke(); OnSliderRechargeStationDeath?.Invoke(); OnTurretPillarsDeath?.Invoke();  } },
        {9, () =>{ OnControlHeadsDeath?.Invoke(); OnHarpoonControlHeadsDeath?.Invoke(); OnSliderRechargeHeadsDeath?.Invoke(); OnTurretStationsDeath?.Invoke();  OnHarpoonHeadDeath?.Invoke(); OnSliderHeadDeath?.Invoke();  } },
        {10, () => OnTurretHeadsDeath?.Invoke() },
        {11, () =>{ OnShieldAdapterDeath?.Invoke(); OnShieldStorageDeath?.Invoke(); OnShieldEmitterDeath?.Invoke();  OnTokenTransporterDeath?.Invoke();  } },
    };





    void Die()
    {
        IEnumerator StartDeath()
        {

            GameObject[] blockers = GameObject.FindGameObjectsWithTag(Tags.BLOCKER);

            foreach (GameObject blocker in blockers) 
            {
                blocker.transform.position = new Vector3(blocker.transform.position.x, 13, blocker.transform.position.z);
            }




            for (int i = 1; i <= execution_order_dict.Count; i++)
            {
                execution_order_dict[i]();
                yield return new WaitForSeconds(WAIT_TIME);

            }








            SceneManager.LoadScene(2);
        }

        StartCoroutine(StartDeath());


        
    }



    private void OnDestroy()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath -= Die;
    }

    void Start()
    {
        SpinnerChargeUp.OnLaserShotPlayerDeath += Die;
    }

    
}
