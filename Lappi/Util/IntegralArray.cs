namespace Lappi.Util {

    public class IntegralArray<T> where T : new() {

        private readonly T[] integral;

        public IntegralArray (T[] array) {
            integral = Arrays.New<T>(array.Length + 1, (i, v) => i == 0 ? new T() : (dynamic) v[i - 1] + array[i - 1]);
        }

        public T Sum (int left, int right) => left <= right ? (dynamic) integral[right + 1] - integral[left] : new T();

    }

}
