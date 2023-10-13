using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexHolder
{
    public int minParent, maxParent, minChild, maxChild;


    public int child, parent;


    public IndexHolder(int minParent, int maxParent, int minChild, int maxChild)
    {
        this.minParent = minParent;
        this.maxParent = maxParent;
        this.minChild = minChild;
        this.maxChild = maxChild;


        this.parent = minParent;
        this.child = minChild;



    }


    public void HardSetValues(int parent, int child)
    {
        this.parent = parent; this.child = child;
    }

    public int ChangeIndex(int parentDelta, int childDelta)
    {

        child = (childDelta > 0 || parent > minParent - 1) ? child + childDelta : child;
        if (child == maxChild + 1)
        {
            parent++;
            child = minChild;

        }
        else if (child < /*1*/ minChild)
        {
            parent--;
            child = maxChild;
        }




        parent += parentDelta;

        if (parent == maxParent + 1)
        {
            parent = maxParent;
            child = maxChild;


            return 1;

        }

        if (parent == minParent - 1)
        {
            return -1;
        }


        return 0;


    }



}
