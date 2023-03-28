using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // int[] a =
        // {
        //     7, 6, 4, 3, 1
        // };
        // MaxProfit(a);
        int[][] a=new int[2][];
        a[0]=new int[2]{1,2};
        a[1]=new int[2]{3,4};
        MatrixReshape(a,2,4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public int MaxProfit(int[] prices)
    {
        //how to
        int max=0;      
        for (int i = 0; i < prices.Length-1; i++)
        {
            for (int j = i+1; j < prices.Length; j++)
            {
                var temp= prices[j]-prices[i] ;
                if (temp>0&&max<temp)
                    max=temp;
                
            }
        }
        return max;
    }
    public int[][] MatrixReshape(int[][] mat, int r, int c) {
        Queue<int> q = new Queue<int>();
        for (int i = 0; i < mat.Length; i++)
        {
            for (int j = 0; j < mat[0].Length; j++)
            {
                q.Enqueue(mat[i][j]);
            }
        }

        if (mat.Length*mat[0].Length==r*c)
        {
            var temp = new int[r][];
            for (int i = 0; i < r; i++)
            {
                temp[i] = new int[c];
                for (int j = 0; j < c; j++)
                {
                    temp[i][j] = q.Dequeue();
                }
            }

            return temp;
        }
        else
        {

            return mat;
        }

    }
}
