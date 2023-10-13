using OpenCover.Framework.Model;
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

                LaserTurretCommunicationSO1.OnManualTargeting += (bomb) =>
                {
                    _ = StartTargeting(bomb);
                };



                LaserTurretCommunicationSO1.OnAutoTargetingAttempt += (tag) =>
                {
                    BarrageStart(tag);
                };




                OnBarrageStart += () => LaserTurretCommunicationSO1.SetBarraging(false);
                OnBarrageStart += LaserTurretCommunicationSO1.Raise_AutoTargetingSuccess;

                OnBarrageEnd += () => LaserTurretCommunicationSO1.SetBarraging(false);

                OnTargetingStart += () => LaserTurretCommunicationSO1.Raise_TargetingStart();
                OnTargetingEnd += () => LaserTurretCommunicationSO1.Raise_TargetingEnd();

                break;



            case 2:



                LaserTurretCommunicationSO2.OnManualTargeting += (bomb) =>
                {
                    _ = StartTargeting(bomb);
                };



                LaserTurretCommunicationSO2.OnAutoTargetingAttempt += (tag) =>
                {
                    BarrageStart(tag);
                };




                OnBarrageStart += () => LaserTurretCommunicationSO2.SetBarraging(false);
                OnBarrageStart += LaserTurretCommunicationSO2.Raise_AutoTargetingSuccess;

                OnBarrageEnd += () => LaserTurretCommunicationSO2.SetBarraging(false);

                OnTargetingStart += () => LaserTurretCommunicationSO2.Raise_TargetingStart();
                OnTargetingEnd += () => LaserTurretCommunicationSO2.Raise_TargetingEnd();


                break;


        }


        






        turret_head_charge = transform.GetChild(0).gameObject;









        OnBarrageStart += () => isBarraging = true;
        OnBarrageEnd += () => isBarraging = false;

    }



    public void SetupLaser()
    {



        laser = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(laser.GetComponent<Collider>());
        laser.GetComponent<Renderer>().sharedMaterial = turret_head_charge.GetComponent<Renderer>().material;
        laser.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        originVector = turret_head_charge.transform.position;


        isTargeting = true;













    }



    public async Task StartTargeting(GameObject Bomb)
    {
        try
        {


            OnTargetingStart?.Invoke();
            SetupLaser();

            Task t1 = Target(Bomb);

            Task t2 = Bomb.transform.GetComponent<DamageBomb>().StartDamage(true);

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
            catch (Exception)
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






    /*
        public void EngageBarrageStart(string tag)
        {


           // Debug.Log("isBarraging " + isBarraging);

            if (!isBarraging)
            {
               BarrageStart(tag);

            }
        }
    */
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
            await StartTargeting(target);



            UICommunicationSO.Raise_ScoreChange(1);




        }
    }







    GameObject[] GetAllPossibleTargets(string tag)
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag(tag);
        Material mat = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;


        var coloredTargets = (
            from bomb in allTargets
            where bomb.GetComponent<BombColorChange>().Color.color == mat.color
            where bomb.GetComponent<BombColorChange>().IsNotCurrentlyTargeted()
            select bomb
        );



        return coloredTargets.ToArray();
    }
}


