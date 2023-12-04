namespace ThirdTask
{
    class JuliaFractal : IFractal
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public JuliaFractal(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Compute()
        {
            // Обчислення фракталу Джуліа
            // ...
        }
    }
}
