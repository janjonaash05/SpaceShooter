using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexHolder
{
    public int minParent, maxParent, minChild, maxChild;



    public int Child { get; protected set; }

    public int Parent { get; protected set; }


















    public IndexHolder(int minParent, int maxParent, int minChild, int maxChild)
    {
        this.minParent = minParent;
        this.maxParent = maxParent;
        this.minChild = minChild;
        this.maxChild = maxChild;


        this.Parent = minParent;
        this.Child = minChild;



    }


    public void HardSetValues(int parent, int child)
    {
        this.Parent = parent; this.Child = child;
    }





    


    



    public virtual int ChangeIndex(int parentDelta, int childDelta)
    {
        
        Child = (childDelta > 0 || Parent > minParent - 1) ? Child + childDelta : Child;

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


            Parent = maxParent;
            Child = maxChild;


            return 1;

        }

        if (Parent == minParent - 1)
        {
            
            Parent = minParent;
            Child = minChild;
           


            return -1;
        }



       
        return 0;


    }



}
