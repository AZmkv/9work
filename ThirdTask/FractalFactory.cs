namespace ThirdTask
{
    // фабрика для створення фракталів
    class FractalFactory
    {
        public IFractal CreateFractal(string type, int width, int height)
        {
            switch (type)
            {
                case "Mandelbrot":
                    return new MandelbrotFractal(width, height);
                case "Julia":
                    return new JuliaFractal(width, height);
                default:
                    throw new ArgumentException("Невідомий тип фракталу");
            }
        }
    }

}
