using System;
using System.Threading;

namespace Lab1Multithreading
{
    class SequenceThread
    {
        private readonly int threadId;
        private readonly int step;
        private volatile bool canStop = false;

        public SequenceThread(int id, int step)
        {
            this.threadId = id;
            this.step = step;
        }

        public void StopThread()
        {
            canStop = true;
        }

        public void Run()
        {
            long sum = 0;
            long count = 0;
            long current = 0;

            // Потік працює, поки керуючий потік (Main) не змінить canStop
            while (!canStop)
            {
                sum += current;
                current += step;
                count++;
            }

            Console.WriteLine($"Потік {threadId}: Сума = {sum}, Кількість доданків = {count}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int numThreads = 4; // Кількість потоків можна змінювати
            SequenceThread[] sequenceThreads = new SequenceThread[numThreads];
            Thread[] threads = new Thread[numThreads];

            // Ініціалізація та запуск потоків
            for (int i = 0; i < numThreads; i++)
            {
                // Крок для прикладу дорівнює номеру потоку (i + 1)
                sequenceThreads[i] = new SequenceThread(i + 1, i + 1);
                threads[i] = new Thread(sequenceThreads[i].Run);
                threads[i].Start();
            }

            // Керуючий потік чекає заданий проміжок часу (наприклад, 1 секунда)
            Thread.Sleep(1000);

            // Генерування дозволу на завершення роботи для кожного потоку окремо
            for (int i = 0; i < numThreads; i++)
            {
                sequenceThreads[i].StopThread();
            }

            // Очікування повного завершення всіх потоків (опціонально, але гарна практика)
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].Join();
            }
        }
    }
}