namespace SecondTask
{

    // Клас для передачі додаткових даних за допомогою події
    class SearchCompletedEventArgs : EventArgs
    {
        public int Index { get; set; }

        public SearchCompletedEventArgs(int index)
        {
            Index = index;
        }
    }

}
