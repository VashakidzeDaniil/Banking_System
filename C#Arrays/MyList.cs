using System.Collections;

namespace C_Arrays
{
    public class MyList<T> : IEnumerable <T>
    {
        private T[] items = new T[4];
        private int count;
        public int Count => count;
        public void Add(T value)
        {
            if (count == items.Length)
            {
                T[] bigger = new T[items.Length * 2];
                Array.Copy(items, bigger, count);
                items = bigger;
            }
            items[count] = value;
            count++;
        }
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return items[index];
            }
            set
            {
                if (index < 0 || index >= count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                items[index] = value;
            }
        }
        public bool Contains(T value)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(items[i], value))
                    return true;
            }
            return false;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class Enumerator : IEnumerator<T>
        {
            private readonly MyList<T> list;
            private int index = -1;
            public Enumerator(MyList<T> list) => this.list = list;
            public T Current => list[index];
            object IEnumerator.Current => Current!;
            public bool MoveNext()
            {
                index++;
                return index < list.count;
            }
            public void Reset() => index = -1;
            public void Dispose() { }
        }
    }
}