// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2019-10-22 16:20:49
// * @Description: 双队列(多线程编程中，能保证生产者线程的写入和消费者的读出尽量做到最低的影响，避免了共享队列的锁开销)
// --------------------------------------------------------------------------------

using System.Collections;

namespace UFramework.Core
{
    public class DoubleQueue<T> where T : class
    {
        private Queue _consume;
        private Queue _produce;
        
        public int Count => _produce.Count;

        public DoubleQueue(int capcity = 16)
        {
            _consume = new Queue(capcity);
            _produce = new Queue(capcity);
        }
        
        public void Enqueue(T arg)
        {
            lock (_produce)
            {
                _produce.Enqueue(arg);
            }
        }

        public T Dequeue() => _consume.Dequeue() as T;

        public void Swap()
        {
            lock (_produce)
            {
                if (_produce.Count <= 0)
                    return;

                var temp = _consume;
                _consume = _produce;
                _produce = temp;
            }
        }

        public void Clear()
        {
            lock (_produce)
            {
                _produce.Clear();
                _consume.Clear();
            }
        }

        public bool IsEmpty() => _consume.Count == 0;
    }
}