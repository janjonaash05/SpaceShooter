using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;

public class SpinnerIndexHolder : IndexHolder
{

    const int spinnerMaxParent = 4, spinnerMinParent = 1, spinnerMaxChild = 7, spinnerMinChild = 1;
   // public int parent;

   // public int child;


    static Dictionary<string, int> matDict;


   // int maxParent = 4, maxChild = 7;
     

    public SpinnerIndexHolder(int parent, int child) : base(spinnerMinParent, spinnerMaxParent, spinnerMinChild, spinnerMaxChild)
    {

       
        this.parent = parent;
        this.child = child;

    }


     



    public static List<int> AllMatIndexesByHolder(SpinnerIndexHolder holder, bool color)
    {


        int a = (color) ? -1 : 1;
        var list = new List<int>();

        var copyHolder = new SpinnerIndexHolder(holder.parent, holder.child);

        int changeResult = 0;
        while (changeResult != a)
        {


            list.Add(GetMatIndexByHolder(copyHolder));

            changeResult = copyHolder.ChangeIndex(0, a);

        }






        return list;
    }





    static int GetMatIndexByHolder(SpinnerIndexHolder holder)
    {

        // Debug.Log("GetBy " + holder.parent.ToString() + holder.child.ToString());
        if (holder.parent != 0)
        {
            return matDict[holder.parent.ToString() + holder.child.ToString()];
        }
        else
        {

            return -1;
        }

    }

    public static void LoadMap()
    {
        matDict = new Dictionary<string, int>
        {
            { "11", 25 }, 
            { "12", 18 },
            { "13", 26 },
            { "14", 22 },
            { "15", 27 },
            { "16", 15 },
            { "17", 8 },
            { "21", 11 },
            { "22", 19 }, 
            { "23", 28 },
            { "24", 23 },
            { "25", 29 },
            { "26", 17 },
            { "27", 4 },
            { "31", 3 },
            { "32", 20 },
            { "33", 30 },
            { "34", 6 },
            { "35", 16 },
            { "36", 10 },
            { "37", 5 },
            { "41", 9 },
            { "42", 21 },
            { "43", 7 },
            { "44", 24 },
            { "45", 13 },
            { "46", 12 },
            { "47", 14 }
        };

    }




}






