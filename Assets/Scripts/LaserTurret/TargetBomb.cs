using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;






public class TargetBomb : MonoBehaviour
{



    GameObject turret_head_charge;

    private Vector3 originVector;

    public bool isTargeting = false;
    public bool isBarraging;

    private GameObject laser;



    public event Action OnTargetingStart, OnTargetingEnd;
    public event Action OnBarrageStart, OnBarrageEnd;




    [SerializeField] int ID;




    private void Start()
    {
        LaserTurretChannel channel = LaserTurretCommunicationChannels.GetChannelByID(ID);


        channel.OnManualTargeting += (bomb) =>
        {
            _ = StartTargeting(bomb, BombDestructionType.MANUAL);
        };



        channel.OnAutoTargetingAttempt += (tag) =>
        {
            BarrageStart(tag);
        };




        OnBarrageStart += () => channel.SetBarraging(false);
        OnBarrageStart += channel.Raise_AutoTargetingSuccess;

        OnBarrageEnd += () => channel.SetBarraging(false);

        OnTargetingStart += () => channel.Raise_TargetingStart();
        OnTargetingEnd += () => channel.Raise_TargetingEnd();



        turret_head_charge = transform.GetChild(0).gameObject;


        OnBarrageStart += () => isBarraging = true;
        OnBarrageEnd += () => isBarraging = false;


    }






    /// <summary>
    /// Plays either TURRET_TARGET_BOMB_1 or TURRET_TARGET_BOMB_2 sound, based on ID.
    /// <para>Invokes OnTargetingStart, calls SetupLaser().</para>
    /// <para>Starts the Target and StartDamage tasks, waits for them both to finish.</para>
    /// <para>If anything fails, destroys the laser.</para>
    /// </summary>
    /// <param name="Bomb"></param>
    /// <param name="bombDestructionType"></param>
    /// <returns></returns>
    public async Task StartTargeting(GameObject Bomb, BombDestructionType bombDestructionType)
    {
        try
        {
            AudioManager.PlayActivitySound(ID == 1 ? AudioManager.ActivityType.TURRET_TARGET_BOMB_1 : AudioManager.ActivityType.TURRET_TARGET_BOMB_2);

            OnTargetingStart?.Invoke();
            SetupLaser();

            Task t1 = Target(Bomb);

            Task t2 = Bomb.transform.GetComponent<DamageBomb>().StartDamage(bombDestructionType);

            await Task.WhenAll(t1, t2);
        }
        catch (Exception)
        {
            Destroy(laser);




        }


    }



    /// <summary>
    /// Gets the bomb's BombColorChange, if it's null returns.
    /// <para>While color change isn't finished:</para>
    /// <para>- Increases i by 1.</para>
    /// <para>- Tries to get the target position as the bomb's position.</para>
    /// <para>- Calls Track() with the target position and i multiplied by a half.</para>
    /// <para>Destroys the laser, sets isTargeting to false and invokes OnTargetingEnd. </para>
    /// </summary>
    /// <param name="bomb"></param>
    /// <returns></returns>
    public async Task Target(GameObject bomb)
    {

        float i = 0;




        BombColorChange bcc;
        bomb.TryGetComponent(out bcc);

        if (bcc == null) { return; }
        while (!bcc.Finished)
        {

            i++;

            Vector3 targetPos;
            try
            {
                targetPos = bomb.transform.position;
            }
            catch
            {
                continue;
            }




            Track(targetPos, i * 0.5f * Time.deltaTime);
            await Task.Yield();
        }




        Destroy(laser);
        isTargeting = false;


        OnTargetingEnd?.Invoke();


    }





    /// <summary>
    /// Creates the laser as a cylinder.
    /// <para>Deletes its collider, sets its material as the turret head charge material.</para>
    /// <para>Sets its localScale and assigns the originVector to the turret head charge position.</para>
    ///
    /// </summary>
    public void SetupLaser()
    {



        laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser.GetComponent<Collider>());
        laser.GetComponent<Renderer>().sharedMaterial = turret_head_charge.GetComponent<Renderer>().material;
        laser.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        originVector = turret_head_charge.transform.position;


        //  isTargeting = true;


    }



    /// <summary>
    /// If the laser is null, sets isTargeting to false and invokes OnTargetingEnd.
    /// <para>Calculates the distance from origin to target vectors.</para>
    /// <para>Sets the localScale based on the distance and arg sizeIncrease.</para>
    /// <para>Calculates the middle vector and sets the laser's position to it.</para>
    /// <para>Calculates the rotation direction to face the target, assigns the laser's up rotation to it.</para>
    /// </summary>
    /// <param name="targetVector"></param>
    /// <param name="sizeIncrease"></param>
    void Track(Vector3 targetVector, float sizeIncrease)
    {

        if (laser == null)
        {
            isTargeting = false;


            OnTargetingEnd?.Invoke();
            return;
        }
        float distance = Vector3.Distance(originVector, targetVector);

        laser.transform.localScale = new Vector3(laser.transform.localScale.x + sizeIncrease, distance * 0.5f, laser.transform.localScale.z + sizeIncrease);

        Vector3 middleVector = 0.5f * (originVector + targetVector);
        laser.transform.position = middleVector;

        Vector3 rotationDirection = (targetVector - originVector);
        laser.transform.up = rotationDirection;

    }







    /// <summary>
    /// Gets the targets based on the arg tag.
    /// <para>Returns if there are none.</para>
    /// <para>Invokes OnBarrageStart, awaits Barrage with the targets, invokes OnBarrageEnd.</para>
    /// </summary>
    /// <param name="tag"></param>
    public async void BarrageStart(string tag)
    {

        GameObject[] targets = GetAllPossibleTargets(tag);
        if (targets.Count() == 0) { return; }

        OnBarrageStart?.Invoke();
        await Barrage(targets);
        OnBarrageEnd?.Invoke();
    }



    /// <summary>
    /// For each target awaits StartTargeting with the AUTO BombDestructionType, calls Raise_ScoreChange with 1 on UICommunication.
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    async Task Barrage(GameObject[] targets)
    {
        foreach (var target in targets)
        {
            await StartTargeting(target, BombDestructionType.AUTO);



            UICommunication.Raise_ScoreChange(1);




        }
    }






    /// <summary>
    /// Gets all gameObjects that match the arg tag.
    /// <para>Gets the ChargeColorName, then proceeds to filter the gameObject that have the matching BombColorName and aren't currenty targeted.</para>
    /// </summary>
    /// <param name="tag"></param>
    /// <returns>The filtered gameObjects</returns>
    GameObject[] GetAllPossibleTargets(string tag)
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag(tag);

        COLOR colorName = LaserTurretCommunicationChannels.GetChannelByID(ID).ChargeColorName;


        var coloredTargets = (
            from bomb in allTargets
            where colorName == bomb.GetComponent<BombColorChange>().BombColorName
            where bomb.GetComponent<BombColorChange>().IsNotCurrentlyTargeted()
            select bomb
        ); ;



        return coloredTargets.ToArray();
    }
}


