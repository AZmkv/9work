using SecondTask;
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        int[] array = GenerateArray(1000000);

        SearchObserver observer = new SearchObserver();

        observer.SearchCompleted += Observer_SearchCompleted;

        // Запуск паралельного пошуку
        ParallelSearch(array, observer);

        Console.ReadLine();
    }

    static void ParallelSearch(int[] array, SearchObserver observer)
    {
        int numThreads = Environment.ProcessorCount; 
        int chunkSize = array.Length / numThreads; 

        // Створення та запуск потоків для паралельного пошуку
        Task[] tasks = new Task[numThreads];
        for (int i = 0; i < numThreads; i++)
        {
            int startIndex = i * chunkSize;
            int endIndex = (i == numThreads - 1) ? array.Length : (i + 1) * chunkSize;

            tasks[i] = Task.Run(() => Search(array, startIndex, endIndex, observer));
        }

        Task.WaitAll(tasks);
    }

    static void Search(int[] array, int startIndex, int endIndex, SearchObserver observer)
    {
        int target = 42; 

        for (int i = startIndex; i < endIndex; i++)
        {
            if (array[i] == target)
            {
                observer.OnElementFound(i); 
            }
        }
    }

    static int[] GenerateArray(int size)
    {
        Random random = new Random();
        int[] array = new int[size];

        for (int i = 0; i < size; i++)
        {
            array[i] = random.Next(1, 100);
        }

        return array;
    }

    static void Observer_SearchCompleted(object sender, SearchCompletedEventArgs e)
    {
        Console.WriteLine("Пошук завершено. Знайдено елемент на позиції: " + e.Index);
    }
}