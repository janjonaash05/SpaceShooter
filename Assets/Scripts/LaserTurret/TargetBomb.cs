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




        switch (ID)
        {
            case 1:

                LaserTurretCommunicationChannels.Channel1.OnManualTargeting += (bomb) =>
                {
                    _ = StartTargeting(bomb, BombDestructionType.MANUAL);
                };



                LaserTurretCommunicationChannels.Channel1.OnAutoTargetingAttempt += (tag) =>
                {
                    BarrageStart(tag);
                };




                OnBarrageStart += () => LaserTurretCommunicationChannels.Channel1.SetBarraging(false);
                OnBarrageStart += LaserTurretCommunicationChannels.Channel1.Raise_AutoTargetingSuccess;

                OnBarrageEnd += () => LaserTurretCommunicationChannels.Channel1.SetBarraging(false);

                OnTargetingStart += () => LaserTurretCommunicationChannels.Channel1.Raise_TargetingStart();
                OnTargetingEnd += () => LaserTurretCommunicationChannels.Channel1.Raise_TargetingEnd();

                break;



            case 2:



                LaserTurretCommunicationChannels.Channel2.OnManualTargeting += (bomb) =>
                {
                    _ = StartTargeting(bomb, BombDestructionType.MANUAL);
                };



                LaserTurretCommunicationChannels.Channel2.OnAutoTargetingAttempt += (tag) =>
                {
                    BarrageStart(tag);
                };




                OnBarrageStart += () => LaserTurretCommunicationChannels.Channel2.SetBarraging(false);
                OnBarrageStart += LaserTurretCommunicationChannels.Channel2.Raise_AutoTargetingSuccess;

                OnBarrageEnd += () => LaserTurretCommunicationChannels.Channel2.SetBarraging(false);

                OnTargetingStart += () => LaserTurretCommunicationChannels.Channel2.Raise_TargetingStart();
                OnTargetingEnd += () => LaserTurretCommunicationChannels.Channel2.Raise_TargetingEnd();


                break;


        }


        






        turret_head_charge = transform.GetChild(0).gameObject;









        OnBarrageStart += () => isBarraging = true;
        OnBarrageEnd += () => isBarraging = false;


    }



    



    public async Task StartTargeting(GameObject Bomb, BombDestructionType bombDestructionType)
    {
        try
        {
            AudioManager.PlayActivitySound( ID==1 ?   AudioManager.ActivityType.TURRET_TARGET_BOMB_1 : AudioManager.ActivityType.TURRET_TARGET_BOMB_2);

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






    public void SetupLaser()
    {



        laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser.GetComponent<Collider>());
        laser.GetComponent<Renderer>().sharedMaterial = turret_head_charge.GetComponent<Renderer>().material;
        laser.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        originVector = turret_head_charge.transform.position;


      //  isTargeting = true;


    }


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






   
    public async void BarrageStart(string tag)
    {

        GameObject[] targets = GetAllPossibleTargets(tag);
        if (targets.Count() == 0) { return; }

        OnBarrageStart?.Invoke();
        await Barrage(targets);
        OnBarrageEnd?.Invoke();
    }

    async Task Barrage(GameObject[] targets)
    {
        foreach (var target in targets)
        {
            await StartTargeting(target, BombDestructionType.AUTO);



            UICommunication.Raise_ScoreChange(1);




        }
    }







    GameObject[] GetAllPossibleTargets(string tag)
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag(tag);
        Material mat = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;


        COLOR colorName = LaserTurretCommunicationChannels.GetChannelByID(ID).ChargeColorName;


        var coloredTargets = (
            from bomb in allTargets
                // where mat.name.Contains(bomb.GetComponent<BombColorChange>().BombMaterial.name) ||  bomb.GetComponent<BombColorChange>().BombMaterial.name.Contains(mat.name)
            where colorName == bomb.GetComponent<BombColorChange>().BombColorName
            where bomb.GetComponent<BombColorChange>().IsNotCurrentlyTargeted()
            select bomb
        ); ;



        return coloredTargets.ToArray();
    }
}


