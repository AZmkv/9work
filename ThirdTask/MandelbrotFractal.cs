namespace ThirdTask
{
    // реалізація фракталу Мандельброта
    class MandelbrotFractal : IFractal
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public MandelbrotFractal(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Compute()
        {
            // Обчислення фракталу Мандельброта
            // ...
        }
    }
}
