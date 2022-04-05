namespace ArrySorter
{
    public class SortNumbers
    {
        private float[] numbers;

        public SortNumbers(float[] numbers)
        {
            this.numbers = numbers;
        }
        
        public void SortAufInt()
        {
            float tmp = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length - 1; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        tmp = numbers[j + 1];
                        numbers[j + 1] = numbers[j];
                        numbers[j] = tmp;
                    }
                }
            }
        }
        
        public void SortAbInt()
        {
            float tmp = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length - 1; j++)
                {
                    if (numbers[j] < numbers[j + 1])
                    {
                        tmp = numbers[j + 1];
                        numbers[j + 1] = numbers[j];
                        numbers[j] = tmp;
                    }
                }
            }
        }
    }
}