namespace SecondTask
{
    // Клас спостерігач, який генерує подію про знайдений елемент
    class SearchObserver
    {
        public event EventHandler<SearchCompletedEventArgs> SearchCompleted;
        public void OnElementFound(int index)
        {
            SearchCompleted?.Invoke(this, new SearchCompletedEventArgs(index));
        }
    }
}
