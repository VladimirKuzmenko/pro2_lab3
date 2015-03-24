using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

/**
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *                                                             *
 *            Parallel and Distributed Computing               *
 *             Laboratory work #3. C#                          *
 *                                                             *
 * Task: A = sort(B)*α +  β*Z*(MO*MK)                          *
 *                                                             *
 * File Program.cs                                             *
 * @author Kuzmenko Volodymyr                                  *
 * @group IO-21                                                *
 * @date 24.03.15                                              *
 *                                                             *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 */

namespace pro2_lab3
{
    class Program : Operations
    {
        public const int N = 1600;
        public const int P = 4;
        public const int H = N / P;

        public static EventWaitHandle Evn4_123;
        public static EventWaitHandle Evn4_B;

        public static object lock_Z = new object();
        public static object monitor_MO = new object();

        public static Mutex mutex_beta = new Mutex(false);

        public static Semaphore sem2_1, sem1_4, sem3_4, sem2A, sem1A, sem3A;

        public static volatile int alpha;
        public static int beta;

        public static Vector A = new Vector(N);
        public static Vector B = new Vector(N);
        public static Vector Z = new Vector(N);

        public static Matrix MO = new Matrix(N);
        public static Matrix MK = new Matrix(N);


        /********************************* Задача Т1 *********************************/
        public static void T1()
        {
            Console.WriteLine("Task 1 started");
            /*1. Введення MK, α*/
            alpha = 1;
            MK = inputMatrix(N,1);
            /*2. Чекати на введення Z, B в T4*/
            Evn4_123.WaitOne();
            /*3. Обчислення BH = sort(BH)*/
            Vector buf = new Vector(H);
            buf = sort(B, 0, H);
            for (int i = 0; i < H; i++)
            {
                B.set(i, buf.get(i));
            }
            /*4. Чекати на завершення обчислень BH в T2*/
            sem2_1.WaitOne();
            /*5. Злиття B2H = mergeSort(BH, BH)*/
            mergeSort(B, 0, 2 * H);
            /*6. Сигнал Т4 про завершення злиття в Т1*/
            sem1_4.Release();
            /*7. Чекати завершення злиття B в Т4*/
            Evn4_B.WaitOne();
            /*8. Копіювати α1 := α*/
            int alpha1 = alpha;
            /*9. Копіювати β1 := β*/
            mutex_beta.WaitOne();
            int beta1 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z1 = new Vector(N);
            /*10. Копіювати Z1 := Z*/
            lock (lock_Z)
            {
                Z1 = Z;
            }

            Matrix MO1 = new Matrix(N);
            /*11. Копіювання MO1 := MO*/
            try
            {
                Monitor.Enter(monitor_MO);
                MO1 = MO;
            }
            finally
            {
                Monitor.Exit(monitor_MO);
            }
            /*12. Обчислення AH = BH∙α1 + β1∙Z1∙(MO1∙MKH) */
            buf = add(mult(alpha1,B,0,H),mult(beta1,mult(Z,mult(MO1, MK,0,H),0,H),0,H),0,H);
            for (int i = 0; i < H; i++)
            {
                A.set(i, buf.get(i));
            }
            /*13. Сигнал Т4 про завершення обчислень*/
            sem1A.Release();
            Console.WriteLine("Task 1 finished");
        }

        /********************************* Задача Т2 *********************************/
        public static void T2()
        {
            Console.WriteLine("Task 2 started");
            /*1. Чекати на введення B, Z у задачі T4*/
            Evn4_123.WaitOne();

            Vector buf = new Vector(H);
            /*2. Обчислення BH = sort(BH)*/
            buf = sort(B, H, 2*H);
            for (int i = H; i < 2* H; i++)
            {
                B.set(i, buf.get(i));
            }
            /*3. Сигнал Т1 про завершення сортування  BH */
            sem2_1.Release();
            /*4. Чекати на завершення злиття B в Т4*/
            Evn4_B.WaitOne();
            /*5. Копіювати α2 := α*/
            int alpha2 = alpha;
            /*6. Копіювати β2 := β*/
            mutex_beta.WaitOne();
            int beta2 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z2 = new Vector(N);
            /*7. Копіювати Z2 := Z*/
            lock (lock_Z)
            {
                Z2 = Z;
            }
            Matrix MO2 = new Matrix(N);
            /*8. Копіювання MO2 := MO*/
            try
            {
                Monitor.Enter(monitor_MO);
                MO2 = MO;
            }
            finally
            {
                Monitor.Exit(monitor_MO);
            }
            /*9. Обчислення AH = BH∙α2 + β2∙Z2∙(MO2∙MKH)*/
            buf = add(mult(alpha2, B, H, 2 * H), mult(beta2, mult(Z, mult(MO2, MK, H, 2 * H), H, 2 * H), H, 2 * H), H, 2 * H);
            for (int i = H; i < 2*H; i++)
            {
                A.set(i, buf.get(i));
            }
            /*10. Сигнал Т4 про завершення обчислень*/
            sem2A.Release();

            Console.WriteLine("Task 2 finished");
        }

        /********************************* Задача Т3 *********************************/
        public static void T3()
        {
            Console.WriteLine("Task 3 started");
            /*1. Введення МO, β*/
            beta = 1;
            MO = inputMatrix(N, 1);
            /*2. Чекати на введення B, Z у задачі T4*/
            Evn4_123.WaitOne();

            /*3. Обчислення BH = sort(BH)*/
            Vector buf = new Vector(H);
            buf = sort(B, 2*H, 3 * H);
            for (int i = 2*H; i < 3 * H; i++)
            {
                B.set(i, buf.get(i));
            }
            /*4. Сигнал Т4 про завершення BH*/
            sem3_4.Release();
            /*5. Чекати на завершення злиття B в Т4*/
            Evn4_B.WaitOne();
            /*6. Копіювати α3 := α*/
            int alpha3 = alpha;
            /*7. Копіювати β3 := β*/
            mutex_beta.WaitOne();
            int beta3 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z3 = new Vector(N);
            /*8. Копіювати Z3 := Z*/
            lock (lock_Z)
            {
                Z3 = Z;
            }
            Matrix MO3 = new Matrix(N);
            /*9. Копіювання MO3 := MO*/
            try
            {
                Monitor.Enter(monitor_MO);
                MO3 = MO;
            }
            finally
            {
                Monitor.Exit(monitor_MO);
            }
            /*10. Обчислення AH = BH∙α3 + β3∙Z3∙(MO3∙MKH)*/
            buf = add(mult(alpha3, B, 2*H, 3 * H), mult(beta3, mult(Z, mult(MO3, MK, 2*H, 3 * H), 2*H, 3 * H), 2*H, 3 * H),2*H, 3 * H);
            for (int i = 2*H; i < 3 * H; i++)
            {
                A.set(i, buf.get(i));
            }
            /*11. Сигнал Т4 про завершення обчислень*/
            sem3A.Release();


            Console.WriteLine("Task 3 finished");
        }

        /********************************* Задача Т4 *********************************/
        public static void T4()
        {
            Console.WriteLine("Task 4 started");
            /*1. Введення B, Z*/
            B = inputVector(N , 1);
            Z = inputVector(N, 1);
            /*2. Сигнал задачам T1, T2, T3 про введення B, Z*/
            Evn4_123.Set();

            Vector buf = new Vector(H);
            /*3. Обчислення BH = sort(BH)*/
            buf = sort(B, 3 * H, N);
            for (int i = 3 * H; i < N; i++)
            {
                B.set(i, buf.get(i));
            }
            /*4. Чекати на завершення обчислень BH в T3*/
            sem3_4.WaitOne();
            /*5. Злиття B2H = mergeSort(BH, BH)*/
            mergeSort(B, 2*H, N);
            /*6. Чекати завершення злиття в Т1*/
            sem1_4.WaitOne();
            /*7. B = mergeSort(B2H, B2H)*/
            mergeSort(B, 0, N);
            /*8. Сигнал задачам T1, T2, Т3 про завершення злиття B*/
            Evn4_B.Set();
            /*9. Копіювати α4 := α*/
            int alpha4 = alpha;
            /*10. Копіювати β4 := β*/
            mutex_beta.WaitOne();
            int beta4 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z4 = new Vector(N);
            /*11. Копіювати Z4 := Z*/
            lock (lock_Z)
            {
                Z4 = Z;
            }
            /*12. Копіювання MO4 := MO*/
            Matrix MO4 = new Matrix(N);
            try
            {
                Monitor.Enter(monitor_MO);
                MO4 = MO;
            }
            finally
            {
                Monitor.Exit(monitor_MO);
            }
            /*13. Обчислення AH = BH∙α4 + β4∙Z4∙(MO4∙MKH)*/
            buf = add(mult(alpha4, B, 3 * H, N), mult(beta4, mult(Z, mult(MO4, MK, 3 * H, N), 3 * H, N), 3 * H, N), 3 * H, N);
            for (int i = 3 * H; i < N; i++)
            {
                A.set(i, buf.get(i));
            }
            /*14. Чекати завершення обчислень AH в задачах T1, T2, T3*/
            sem1A.WaitOne();
            sem2A.WaitOne();
            sem3A.WaitOne();
            /*15. Вивести A*/
            outputVector(A);
            Console.WriteLine("Task 4 finished");
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine("Lab3 started");

            Evn4_123 = new ManualResetEvent(false);
            Evn4_B = new ManualResetEvent(false);

            sem1_4 = new Semaphore(0, 1);
            sem2_1 = new Semaphore(0, 1);
            sem3_4 = new Semaphore(0, 1);
            sem1A = new Semaphore(0, 1);
            sem2A = new Semaphore(0, 1);
            sem3A = new Semaphore(0, 1);

            Thread t1 = new Thread(T1);
            Thread t2 = new Thread(T2);
            Thread t3 = new Thread(T3);
            Thread t4 = new Thread(T4);

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            System.Console.WriteLine("Lab3 finished");

        }
    }
}
