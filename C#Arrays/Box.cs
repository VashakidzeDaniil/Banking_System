namespace C_Arrays
{
    public class Box<T>
    {
        private T? item;

        public void Put(T value) => item = value;
        public T Get() => item;
        public bool IsEmpty => item is null;
    }

}
