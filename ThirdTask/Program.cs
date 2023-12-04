using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using ThirdTask;

class Program
{
    static void Main(string[] args)
    {
        FractalObserver observer = new FractalObserver();

        observer.ComputationProgress += Observer_ComputationProgress;

        FractalFactory factory = new FractalFactory();

        IFractal mandelbrot = factory.CreateFractal("Mandelbrot", 800, 600);
        IFractal julia = factory.CreateFractal("Julia", 800, 600);

        Parallel.Invoke(() => ComputeFractal(mandelbrot, observer),
                        () => ComputeFractal(julia, observer));

        Console.ReadLine();
    }

    static void ComputeFractal(IFractal fractal, FractalObserver observer)
    {
        // Обчислення фракталу
        fractal.Compute();

        observer.OnComputationProgress();
    }

    static void Observer_ComputationProgress(object sender, EventArgs e)
    {
        Console.WriteLine("Обчислення фракталу завершено");
    }
}

// Інтерфейс для фракталів
interface IFractal
{
    int Width { get; }
    int Height { get; }

    void Compute();
}
