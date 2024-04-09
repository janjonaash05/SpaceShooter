using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class MaterialIndexHolder : IndexHolder
{

    const int max_parent = 4, min_parent = 1, max_child = 4, min_child = 1;




    public enum Edge { UPPER, LOWER, NONE }



    public Edge edge { get; protected set; }
    public void SetEdge(Edge edge) => this.edge = edge;

    private int child, parent;

    public new int Child
    {



        get
        {

            return edge switch
            {
                Edge.UPPER => minChild,
                Edge.LOWER => maxChild,
                Edge.NONE => child


            };
        }




        protected set { child = value; }
    }





    public new int Parent
    {



        get
        {

            return edge switch
            {
                Edge.UPPER => maxParent + 1,
                Edge.LOWER => minParent - 1,
                Edge.NONE => parent


            };
        }




        set { parent = value; }
    }








    static readonly Dictionary<string, int> spinner_mat_dict = new()
        {
            { "11", 18 },
            { "12", 17 },
            { "13", 16 },
            { "14", 15 },
            { "21", 14 },
            { "22", 13 },
            { "23", 12 },
            { "24", 11 },
            { "31", 10 },
            { "32", 9 },
            { "33", 8 },
            { "34", 7 },
            { "41", 6 },
            { "42", 5 },
            { "43", 3 },
            { "44", 4 },
        };
    static readonly Dictionary<string, int> core_mat_dict = new()
        {


            { "11", 2 },
            { "12", 12 },
            { "13", 7 },
            { "14", 11 },
            { "21", 10 },
            { "22", 6 },
            { "23", 4 },
            { "24", 8 },
            { "31", 9 },
            { "32", 17 },
            { "33", 5 },
            { "34", 16 },
            { "41", 15 },
            { "42", 3 },
            { "43", 14 },
            { "44", 13 },











        };


    public void SetToMax()
    {
        Parent = maxParent;
        Child = maxChild;


        Debug.Log("Setting to max " + target);


    }

    public void SetToMin()
    {
        Parent = minParent;
        Child = minChild;

        Debug.Log("Setting to min " + target);

    }

    public bool IsAtMax() { return Parent == maxParent && Child == maxChild; }
    public bool IsAtMin() { return Parent == minParent && Child == minChild; }

    public override int ChangeIndex(int parentDelta, int childDelta)
    {

        // child = (childDelta > 0 || parent > minParent - 1) ? child + childDelta : child;



        
        if (IsAtMax() && childDelta >= 0) { Debug.Log("max to upper" + target); SetEdge(Edge.UPPER); return 1; }

        if (IsAtMin() && childDelta <= 0) { Debug.Log("min to lower" + target); SetEdge(Edge.LOWER); return -1; }


        if (edge == Edge.UPPER)
        {

            if (childDelta <= 0)
            {
                SetEdge(Edge.NONE);
                SetToMax();
                return 0;
            }
            else return 1;
        }

        if (edge == Edge.LOWER)
        {

            if (childDelta >= 0)
            {
                SetEdge(Edge.NONE);
                SetToMin();

                return 0;
            }
            else return -1;

        }



        Child += childDelta;

        if (Child >= maxChild + 1)
        {
            Parent++;
            Child = minChild;

        }
        else if (Child < minChild)
        {
            Parent--;
            Child = maxChild;
        }




        Parent += parentDelta;

        if (Parent >= maxParent + 1)
        {
            SetEdge(Edge.UPPER);
            SetToMax();
            return 1;

        }

        if (parent == minParent - 1)
        {
            SetEdge(Edge.LOWER);
            SetToMin();
            return -1;
        }





        return 0;


    }


    public override string ToString()
    {
        return Parent + ", " + Child + ", " + target + ", " + edge;
    }


    Target target;

    public MaterialIndexHolder(int parent, int child, Target target, Edge edge) : base(min_parent, max_parent, min_child, max_child)
    {


        this.Parent = parent;
        this.Child = child;
        this.target = target;
        this.edge = edge;
    }

    public enum Target { CORE, SPINNER }




    public List<int> AllMatIndexesByHolder(bool color)
    {


        //if ((IsAtMax() && color == false) || (IsAtMin() && color == true)) { Debug.Log("return empty because color is"+ color); return new(); }

        int direction = (color) ? -1 : 1;




        var list = new List<int>();

        var copyHolder = new MaterialIndexHolder(Parent, Child, target, edge);

        int changeResult = 0;
        while (changeResult != direction)
        {




            list.Add(GetCurrentMatIndex(copyHolder));

            changeResult = copyHolder.ChangeIndex(0, direction);
            //  Debug.Log(copyHolder);

        }




        list.Remove(-1);

        return list;
    }





    int GetCurrentMatIndex(MaterialIndexHolder holder)
    {

        if (holder.Parent == minParent - 1 || holder.Parent == maxParent + 1) return -1;



        return target == Target.SPINNER ?
            spinner_mat_dict[holder.Parent.ToString() + holder.Child.ToString()]
            : core_mat_dict[holder.Parent.ToString() + holder.Child.ToString()];



    }





    





}



