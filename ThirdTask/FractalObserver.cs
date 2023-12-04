namespace ThirdTask
{
    // Клас спостерігач, який генерує подію про прогрес обчислення фракталів
    class FractalObserver
    { 
        public event EventHandler ComputationProgress;

        public void OnComputationProgress()
        {
            ComputationProgress?.Invoke(this, EventArgs.Empty);
        }
    }

}
