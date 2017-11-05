namespace Lappi.Filter.Digital {

    public class IntegralArray<T> where T : new() {

        private readonly T[] integral;

        public IntegralArray (T[] array) {
            integral = new T[array.Length + 1];
            for( int i = 1; i < integral.Length; i++ ) {
                integral[i] = (dynamic) integral[i - 1] + array[i - 1];
            }
        }

        public T Sum (int left, int right) => left <= right ? (dynamic) integral[right + 1] - integral[left] : new T();

    }

}
