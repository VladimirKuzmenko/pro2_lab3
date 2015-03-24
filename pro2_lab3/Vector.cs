using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *                                                             *
 *            Parallel and Distributed Computing               *
 *             Laboratory work #3. C#                          *
 *                                                             *
 * Task: A = sort(B)*α +  β*Z*(MO*MK)                          *
 *                                                             *
 * File Vector.cs                                              *
 * @author Kuzmenko Volodymyr                                  *
 * @group IO-21                                                *
 * @date 24.03.15                                              *
 *                                                             *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 */

namespace pro2_lab3
{
    class Vector
    {
        private int[] array;

        public Vector(int n)
        {
            array = new int[n];
        }

        public void set(int index, int value)
        {
            array[index] = value;
        }

        public int get(int index)
        {
            return array[index];
        }

        public int size()
        {
            return array.Length;
        }

        public String toString()
        {
            String res = "";
            for (int i = 0; i < array.Length; i++)
            {
                res += "   " + array[i];
            }
            return res;
        }


    }
}
