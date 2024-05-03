using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;


public class ShieldRecharge : AbstractChargeRecharge
{




















    ParticleSystem ps;

    [SerializeField] GameObject shield_emitter, shield_emitter_antenna;
    Renderer emitter_rend, antenna_rend;

    Material on_mat, off_mat;

    protected override float POSITION_FOR_SIZE_DIVIDER { get => 100; }

    protected override Func<float, Vector3> FormPlacementVector => (x) => new(0,x,0);

    protected override AudioManager.ActivityType CHARGE_SPAWN_SOUND_ACTIVITY_TYPE => AudioManager.ActivityType.SHIELD_CHARGE_SPAWN;

    protected override ParticleSystem GetParticleSystem =>ps;

    protected override List<GameObject> GetChargesAsSortedList() => charges.OrderByDescending(x => x.transform.localPosition.y).ToList();

    private void OnDestroy()
    {

        CoreCommunication.OnBombFallen -= BombFallen;
        CoreCommunication.OnShieldDepleted -= RechargeOnDepletion;
        UpgradesManager.OnShieldMaxCapacityValueChange -= ShieldMaxCapacityValueChange;
    }





    void Start()
    {


        max_capacity = UpgradesManager.SHIELD_MAX_CAPACITY;

        charges = new();
        StartCoroutine(GenerateCharges());
        emitter_rend = shield_emitter.GetComponent<Renderer>();

        on_mat = emitter_rend.materials[2];
        off_mat = emitter_rend.materials[1];


        antenna_rend = shield_emitter_antenna.GetComponent<Renderer>();


        ps = transform.GetComponentInChildren<ParticleSystem>();





        CoreCommunication.OnBombFallen += BombFallen;
        CoreCommunication.OnShieldDepleted += RechargeOnDepletion;
        UpgradesManager.OnShieldMaxCapacityValueChange += ShieldMaxCapacityValueChange;











    }


  

    /// <summary>
    /// If recharging returns. Otherwise sets the charge at SHIELD_CAPACITY's renderer to disabled. 
    /// </summary>
    /// <param name="m"></param>
    void BombFallen(Material m)
    {
        if (recharging) return;
        charges[CoreCommunication.SHIELD_CAPACITY].GetComponent<Renderer>().enabled = false;

    }


    /// <summary>
    /// If recharging, starts UpgradeRecharging coroutine. Else, starts Upgrade coroutine.
    /// </summary>
    void ShieldMaxCapacityValueChange()
    {
        if (recharging)
        {
            StartCoroutine(UpgradeRecharging());
            return;
        }


        StartCoroutine(Upgrade());



    }

  

    /// <summary>
    /// Changes the emitter and antenna materials at a certain index to arg m.
    /// </summary>
    /// <param name="m"></param>
    void ChangeEmitterAndAntennaColor(Material m)
    {
        var e_mats = emitter_rend.materials;
        e_mats[^1] = m;

        var a_mats = antenna_rend.materials;
        a_mats[^1] = m;

        emitter_rend.materials = e_mats;
        antenna_rend.materials = a_mats;

    }

    protected override (float position_unit, Vector3 scale) GetPositionUnitAndScale()
    {
        float start_size = charge_prefab.transform.localScale.x;
        float size = start_size / max_capacity;
        float position_unit = size / (POSITION_FOR_SIZE_DIVIDER);

        float scaled_size = start_size / (max_capacity + (position_unit * max_capacity * POSITION_FOR_SIZE_DIVIDER));

        Vector3 scale = new(start_size, scaled_size, scaled_size);


        return (position_unit, scale);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>UpgradesManager.SHIELD_MAX_CAPACITY</returns>
    protected override int GetMaxCapacityToSetOnGenerate()
    {
      
        return UpgradesManager.SHIELD_MAX_CAPACITY; 
    }

    protected override string GetTagForList()
    {
        return Tags.SHIELD_CHARGE;
    }

    /// <summary>
    /// Calculates based on CORE_INDEX_HOLDER Parent value, returns positive infinity if value is 0.
    /// </summary>
    /// <returns></returns>
    protected override float CalculateRechargeDelay()
    {
       return CoreCommunication.CORE_INDEX_HOLDER.Parent switch
        {
            5 => 0.75f,
            4 => 1f,
            3 => 1.25f,
            2 => 1.5f,
            1 => 1.75f,
            0 => float.PositiveInfinity

        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>UpgradesManager.SHIELD_MAX_CAPACITY, CoreCommunication.SHIELD_CAPACITY</returns>
    protected override (int MaxCapacity, int CurrentCapacity) GetValuesForUpgrade()
    {
        return (UpgradesManager.SHIELD_MAX_CAPACITY, CoreCommunication.SHIELD_CAPACITY);
    }

    protected override void ExecuteBeforeRecharge()
    {
        ChangeEmitterAndAntennaColor(off_mat);
    }

    protected override void ExecuteAfterRecharge()
    {
        CoreCommunication.Raise_ShieldRecharged();
        ChangeEmitterAndAntennaColor(on_mat);
    }


















    /* protected  override IEnumerator Recharge(int skips_amount)
   {
       ARBITRARY_CHARGES_RECHARGED_CAPACITY = 0;
       recharging = true;
       ChangeEmitterAndAntennaColor(off_mat);

       foreach (GameObject charge in charges)
       {
           try
           {
               charge.GetComponent<Renderer>().enabled = false;
           }
           catch
           {
           }
       }



       ps.enableEmission = true;
       yield return StartCoroutine(RegenerateCharges(skips_amount));
       ps.enableEmission = false;



       recharging = false;

       CoreCommunication.Raise_ShieldRecharged();




       ChangeEmitterAndAntennaColor(on_mat);

       ARBITRARY_CHARGES_RECHARGED_CAPACITY = UpgradesManager.SHIELD_MAX_CAPACITY;

   }

   */


    /*
     
    //POSITION_FOR_SCALE_DIVIDER = 100


    /// <summary>
    /// If not recharging, sets the current_recharge_coroutine to the start of the Recharge coroutine with 0 skips.
    /// </summary>
    void RechargeOnDepletion()
    {
        if (!recharging) current_recharge_coroutine = StartCoroutine(Recharge(0));
    }

    
  /// <summary>
  /// <para>Stops the current_recharge_coroutine.</para>
  /// <para>Yields for GenerateCharges coroutine.</para>
  /// <para>Yields and assigns the current_recharge_coroutine to the start Recharge coroutine with ARBITRARY_SHIELD_RECHARGED_CAPACITY as skips amount. </para>
  /// </summary>
  /// <returns></returns>
  IEnumerator UpgradeRecharging()
  {


      StopCoroutine(current_recharge_coroutine);

      yield return StartCoroutine(GenerateCharges());

      yield return current_recharge_coroutine = StartCoroutine(Recharge(ARBITRARY_SHIELD_RECHARGED_CAPACITY));
  }



  /// <summary>
  /// <para>Yields for GenerateCharges coroutine.</para>
  /// <para>Goes from the current capacity value minus 1 to the current SHIELD_CAPACITY, and disables renderers for charges at those indexes.</para>
  /// </summary>
  /// <returns></returns>
  IEnumerator Upgrade()
  {
      yield return StartCoroutine(GenerateCharges());
      int capacity = CoreCommunication.SHIELD_CAPACITY;


      for (int i = UpgradesManager.GetCurrentTurretCapacityValue() - 1; i >= capacity; i--)
      {
          try
          {
              charges[i].GetComponent<Renderer>().enabled = false;
          }
          catch
          {

          }

      }

  }

  /// <summary>
  /// Calculates start size, size, position unit, scaled size and scale.
  /// </summary>
  /// <returns>position_unit, scale</returns>
  (float position_unit, Vector3 scale) GetCalculationValues()
  {
      float start_size = charge_prefab.transform.localScale.x;
      float size = start_size / max_capacity;
      float position_unit = size / (POSITION_FOR_SIZE_DIVIDER);

      float scaled_size = start_size / (max_capacity + (position_unit * max_capacity * POSITION_FOR_SIZE_DIVIDER));

      Vector3 scale = new(start_size, scaled_size, scaled_size);


      return (position_unit, scale);


  }


  /// <summary>
  /// <para>Yields DestroyCharges coroutine.</para>
  /// <para>Assigns max capacity, position unit and scale values.</para>
  /// <para>If max capacity is even, creates charges at locations calculated from both directions from 0, with no center charge.</para>
  /// <para>If max capacity is odd, creates charges at locations calculated from both directions from 0, with a center charge.</para>
  /// </summary>
  /// <returns></returns>
  IEnumerator GenerateCharges()
  {
      yield return StartCoroutine(DestroyCharges());
      max_capacity = UpgradesManager.SHIELD_MAX_CAPACITY;

      (float position_unit, Vector3 scale) = GetCalculationValues();





      if (max_capacity % 2 == 0)
      {
          for (int i = 0; i < 2; i++)
          {
              float mult = i == 0 ? 1 : -1;
              for (int j = 0; j < max_capacity / 2; j++)
              {

                  float pos = position_unit + position_unit * 2 * j;

                  CreateCharge(new(0, pos * mult, 0), scale);
              }

          }
      }
      else
      {
          for (int i = 0; i < 2; i++)
          {
              float mult = i == 0 ? 1 : -1;
              for (int j = 1; j <= (max_capacity - 1) / 2; j++)
              {
                  float pos = position_unit * 2 * j;
                  CreateCharge(new(0, pos * mult, 0), scale);


              }
          }



          CreateCharge(Vector3.zero, scale);

      }


      CreateChargeList();


  }

  /// <summary>
  /// Creates a charges list, adds all children with the SHIELD_CHARGE tag, then orders it descending based on the localPosition y value.
  /// </summary>
  void CreateChargeList()
  {
      charges = new();

      foreach (Transform child in transform)
      {
          if (child.CompareTag(Tags.SHIELD_CHARGE))
          {
              charges.Add(child.gameObject);

          }

      }

      charges = charges.OrderByDescending(x => x.transform.localPosition.y).ToList();
  }

  /// <summary>
  /// Attempts to destroy all charge gameObjects in charges list.
  /// </summary>
  /// <returns></returns>
  IEnumerator DestroyCharges()
  {
      foreach (GameObject charge in charges)
      {
          try
          {
              Destroy(charge);
          }
          catch { }

          yield return null;

      }

  }


  /// <summary>
  /// Creates a charge gameObject and sets its localPosition to arg pos and localScale to arg scale.
  /// </summary>
  /// <param name="pos"></param>
  /// <param name="size"></param>
  void CreateCharge(Vector3 pos, Vector3 size)
  {
      GameObject charge = Instantiate(charge_prefab, transform, false);

      charge.transform.localPosition = pos;
      charge.transform.localScale = size;
  }



  int ARBITRARY_SHIELD_RECHARGED_CAPACITY;



  /// <summary>
  /// <para>Sets the ARBITRARY_SHIELD_RECHARGED_CAPACITY to 0, recharging to true and calls ChangeEmitterAndAntennaColor() with the off material. </para>
  /// <para>Attemps to disable every charge's renderer.</para>
  /// <para>Enables particle system emission, yields for RegenerateCharges with skips amount, disables particle system emission. </para>
  /// <para>Sets recharging to false, calls Raise_ShieldRecharged, calls ChangeEmitterAndAntennaColor() with the on  material.</para>
  /// <para>Sets the ARBITRARY_SHIELD_RECHARGED_CAPACITY to MAX_TURRET_CAPACITY.</para>
  /// </summary>
  /// <param name="skips_amount"></param>
  /// <returns></returns>
  IEnumerator Recharge(int skips_amount)
  {
      ARBITRARY_SHIELD_RECHARGED_CAPACITY = 0;
      recharging = true;
      ChangeEmitterAndAntennaColor(off_mat);

      foreach (GameObject charge in charges)
      {
          try
          {
              charge.GetComponent<Renderer>().enabled = false;
          }
          catch
          {
          }
      }



      ps.enableEmission = true;
      yield return StartCoroutine(RegenerateCharges(skips_amount));
      ps.enableEmission = false;



      recharging = false;

      CoreCommunication.Raise_ShieldRecharged();




      ChangeEmitterAndAntennaColor(on_mat);

      ARBITRARY_SHIELD_RECHARGED_CAPACITY = LaserTurretChannel.MAX_TURRET_CAPACITY;

  }





  /// <summary>
  /// Goes through all charges, increases i and ARBITRARY_SHIELD_RECHARGED_CAPACITY.
  /// <para>For each charge, calculates the recharge delay based on ORE_INDEX_HOLDER Parent value. </para>
  /// <para>Waits the calculated time.</para>
  /// <para>Plays the SHIELD_CHARGE_SPAWN and enables current charge's renderer.</para>
  /// </summary>
  /// <param name="skips_amount"></param>
  /// <returns></returns>
  IEnumerator RegenerateCharges(int skips_amount)
  {
      int i = 0;
      foreach (GameObject charge in charges)
      {
          ARBITRARY_SHIELD_RECHARGED_CAPACITY++;

          i++;
          float recharge_delay = (i <= skips_amount) ? 0 : CoreCommunication.CORE_INDEX_HOLDER.Parent switch
          {
              5 => 0.75f,
              4 => 1f,
              3 => 1.25f,
              2 => 1.5f,
              1 => 1.75f,
              0 => float.PositiveInfinity

          };


          if (float.IsPositiveInfinity(recharge_delay)) { continue; }

          yield return new WaitForSeconds(recharge_delay);

          try
          {
              AudioManager.PlayActivitySound(AudioManager.ActivityType.SHIELD_CHARGE_SPAWN);
              charge.GetComponent<Renderer>().enabled = true;

          }
          catch
          {
          }
      }

  }





  /// <summary>
  /// Changes the emitter and antenna materials at a certain index to arg m.
  /// </summary>
  /// <param name="m"></param>
  void ChangeEmitterAndAntennaColor(Material m)
  {
      var e_mats = emitter_rend.materials;
      e_mats[^1] = m;

      var a_mats = antenna_rend.materials;
      a_mats[^1] = m;

      emitter_rend.materials = e_mats;
      antenna_rend.materials = a_mats;

  }






  */







}
