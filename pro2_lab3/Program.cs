using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace pro2_lab3
{
    class Program : Operations
    {
        public const int N = 8;
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

            alpha = 1;
            MK = inputMatrix(N,1);

            Evn4_123.WaitOne();

            Vector buf = new Vector(H);
            buf = sort(B, 0, H);
            for (int i = 0; i < H; i++)
            {
                B.set(i, buf.get(i));
            }

            sem2_1.WaitOne();
            mergeSort(B, 0, 2 * H);

            sem1_4.Release();
            Evn4_B.WaitOne();

            int alpha1 = alpha;

            mutex_beta.WaitOne();
            int beta1 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z1 = new Vector(N);

            lock (lock_Z)
            {
                Z1 = Z;
            }

            Matrix MO1 = new Matrix(N);
            try
            {
                Monitor.Enter(monitor_MO);
                MO1 = MO;
            }
            finally
            {
                Monitor.Exit(monitor_MO);
            }

            buf = add(mult(alpha1,B,0,H),mult(beta1,mult(Z,mult(MO1, MK,0,H),0,H),0,H),0,H);
            for (int i = 0; i < H; i++)
            {
                A.set(i, buf.get(i));
            }

            sem1A.Release();
            Console.WriteLine("Task 1 finished");
        }

        /********************************* Задача Т2 *********************************/
        public static void T2()
        {
            Console.WriteLine("Task 2 started");
            Evn4_123.WaitOne();

            Vector buf = new Vector(H);
            buf = sort(B, H, 2*H);
            for (int i = H; i < 2* H; i++)
            {
                B.set(i, buf.get(i));
            }

            sem2_1.Release();

            Evn4_B.WaitOne();

            int alpha2 = alpha;

            mutex_beta.WaitOne();
            int beta2 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z2 = new Vector(N);

            lock (lock_Z)
            {
                Z2 = Z;
            }

            Matrix MO2 = new Matrix(N);
            try
            {
                Monitor.Enter(monitor_MO);
                MO2 = MO;
            }
            finally
            {
                Monitor.Exit(monitor_MO);
            }

            buf = add(mult(alpha2, B, H, 2 * H), mult(beta2, mult(Z, mult(MO2, MK, H, 2 * H), H, 2 * H), H, 2 * H), H, 2 * H);
            for (int i = H; i < 2*H; i++)
            {
                A.set(i, buf.get(i));
            }

            sem2A.Release();

            Console.WriteLine("Task 2 finished");
        }

        /********************************* Задача Т3 *********************************/
        public static void T3()
        {
            Console.WriteLine("Task 3 started");

            beta = 1;
            MO = inputMatrix(N, 1);

            Evn4_123.WaitOne();


            Vector buf = new Vector(H);
            buf = sort(B, 2*H, 3 * H);
            for (int i = 2*H; i < 3 * H; i++)
            {
                B.set(i, buf.get(i));
            }

            sem3_4.Release();
            Evn4_B.WaitOne();

            int alpha3 = alpha;

            mutex_beta.WaitOne();
            int beta3 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z3 = new Vector(N);

            lock (lock_Z)
            {
                Z3 = Z;
            }

            Matrix MO3 = new Matrix(N);
            try
            {
                Monitor.Enter(monitor_MO);
                MO3 = MO;
            }
            finally
            {
                Monitor.Exit(monitor_MO);
            }

            buf = add(mult(alpha3, B, 2*H, 3 * H), mult(beta3, mult(Z, mult(MO3, MK, 2*H, 3 * H), 2*H, 3 * H), 2*H, 3 * H),2*H, 3 * H);
            for (int i = 2*H; i < 3 * H; i++)
            {
                A.set(i, buf.get(i));
            }

            sem3A.Release();


            Console.WriteLine("Task 3 finished");
        }

        /********************************* Задача Т4 *********************************/
        public static void T4()
        {
            Console.WriteLine("Task 4 started");

            B = inputVector(N , 1);
            
            Z = inputVector(N, 1);

            Evn4_123.Set();

            Vector buf = new Vector(H);
            buf = sort(B, 3 * H, N);
            for (int i = 3 * H; i < N; i++)
            {
                B.set(i, buf.get(i));
            }

            sem3_4.WaitOne();
            mergeSort(B, 2*H, N);

            sem1_4.WaitOne();
            mergeSort(B, 0, N);

            Evn4_B.Set();

            int alpha4 = alpha;

            mutex_beta.WaitOne();
            int beta4 = beta;
            mutex_beta.ReleaseMutex();

            Vector Z4 = new Vector(N);

            lock (lock_Z)
            {
                Z4 = Z;
            }

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

            buf = add(mult(alpha4, B, 3 * H, N), mult(beta4, mult(Z, mult(MO4, MK, 3 * H, N), 3 * H, N), 3 * H, N), 3 * H, N);
            for (int i = 3 * H; i < N; i++)
            {
                A.set(i, buf.get(i));
            }

            sem1A.WaitOne();
            sem2A.WaitOne();
            sem3A.WaitOne();

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
