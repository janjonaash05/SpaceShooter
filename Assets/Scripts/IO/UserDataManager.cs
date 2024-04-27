using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;



[System.Serializable]
public class UserData
{


    public int[] BestTimeEasy { get; private set; }
    public int[] BestTimeNormal { get; private set; }
    public int[] BestTimeHard { get; private set; }
    public int[] BestScores { get; private set; }
    public float VolumeMultiplier { get; private set; }
    public int[] Resolution { get; private set; }
    public bool Fullscreen { get; private set; }






    public UserData(int[] BestTimeEasy, int[] BestTimeNormal, int[] BestTimeHard, int[] BestScores, float VolumeMultiplier, int[] Resolution, bool Fullscreen)
    {
        this.BestTimeEasy = BestTimeEasy;
        this.BestTimeNormal = BestTimeNormal;
        this.BestTimeHard = BestTimeHard;
        this.BestScores = BestScores;
        this.VolumeMultiplier = VolumeMultiplier;
        this.Resolution = Resolution;
        this.Fullscreen = Fullscreen;
    }


    public void SetBestTimeEasy(int[] var) => BestTimeEasy = var;
    public void SetBestTimeNormal(int[] var) => BestTimeNormal = var;
    public void SetBestTimeHard(int[] var) => BestTimeHard = var;
    public void SetBestScores(int[] var) => BestScores = var;
    public void SetVolumeMultiplier(float var) => VolumeMultiplier = var;
    public void SetResolution(int[] var) => Resolution = var;
    public void SetFullscreen(bool var) => Fullscreen = var;



    public override string ToString()
    {
        return FormatArray(BestTimeEasy) + " " + FormatArray(BestTimeNormal) + " " + FormatArray(BestTimeHard) + " " + FormatArray(BestScores) + " " + VolumeMultiplier + " " + FormatArray(Resolution) + " " + Fullscreen;
    }

    string FormatArray(int[] var)
    {
        string res = "[";
        foreach (int i in var)
        {
            res += i + ",";
        }
        res += "]";

        return res;

    }




};



public static class UserDataManager
{
    // EASY, NORMAL, HARD







    /*
    public static int[][] DIFFICULTY_BEST_TIMES { get; private set; } = new int[3][] { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, };
    public static int[] DIFFICULTY_BEST_SCORES { get; private set; } = new int[3] { 0, 0, 0 };
    public static float VOLUME_MULTIPLIER = 1;
    public static bool FULLSCREEN { get; private set; } = true;

    public static int[] RESOLUTION { get; private set; } = new int[] { Screen.currentResolution.width, Screen.currentResolution.height };
    */




    public static UserData CURRENT_DATA { get; private set; }

    public static void SetSettingsData(SettingsData settings)
    {
        CURRENT_DATA.SetVolumeMultiplier(settings.Volume);
        CURRENT_DATA.SetResolution(settings.Resolution);
        CURRENT_DATA.SetFullscreen(settings.Fullscreen);


        Save();

    }

    public static void ResetUserScoreTime()
    {

      


        CURRENT_DATA.SetBestScores(new int[] { 0, 0, 0 });
        CURRENT_DATA.SetBestTimeEasy(new int[] { 0, 0, 0 });
        CURRENT_DATA.SetBestTimeNormal(new int[] { 0, 0, 0 });
        CURRENT_DATA.SetBestTimeHard(new int[] { 0, 0, 0 });
        Save();

    }

    public static void SetScoreTimeDifficulty(int score, int mins, int secs, int hs, DifficultyManager.Difficulty difficulty)
    {

        try
        {


            (int[] diff_time, int diff_score_index) = difficulty switch
            {
                DifficultyManager.Difficulty.EASY => (CURRENT_DATA.BestTimeEasy, 0),
                DifficultyManager.Difficulty.NORMAL => (CURRENT_DATA.BestTimeNormal, 1),
                DifficultyManager.Difficulty.HARD => (CURRENT_DATA.BestTimeHard, 2),
            };


         


            if (score > CURRENT_DATA.BestScores[diff_score_index])
            {
                CURRENT_DATA.BestScores[diff_score_index] = score;
            }




            int[] time = new int[] { mins, secs, hs };



            
            
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] > diff_time[i])
                {
                    for (int j = i; j < diff_time.Length; j++)
                    {
                        diff_time[j] = time[j];
                    }

                }

            }
            
            

 
            Save();


        }
        catch
        {

        }

    }





    public readonly static string path = Application.persistentDataPath + "/user.data";

    public static void Save()
    {

        try
        {


            BinaryFormatter formatter = new();


            FileStream stream = new(path, FileMode.Create);

            formatter.Serialize(stream, CURRENT_DATA);
            stream.Close();
        }
        catch
        {
        }
    }


    public static void Load()
    {


        /*
        CURRENT_DATA =  GetDefaultData();
        return;
        */




        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);

            try
            {
                  CURRENT_DATA = formatter.Deserialize(stream) as UserData;
            }
            catch
            {

                CURRENT_DATA = GetDefaultData();
            }

            stream.Close();

            return;
        }

        CURRENT_DATA = GetDefaultData();




    }


    public static UserData GetDefaultData() => new
        (
        new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 },
        new int[] { 0, 0, 0 },
        1f, new int[] { Screen.currentResolution.width, Screen.currentResolution.height }, true
        );















}
