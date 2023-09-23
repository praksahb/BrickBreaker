using System;
using System.Collections.Generic;

namespace BrickBreaker
{
    public class GenericPooling<T> where T : class
    {
        private int poolLength;
        private Stack<T> objectPool;
        private Func<T> factoryMethod;

        public GenericPooling(int poolLength, Func<T> factoryMethod)
        {
            this.poolLength = poolLength;
            this.factoryMethod = factoryMethod;

            objectPool = new Stack<T>(poolLength);
            for (int i = 0; i < poolLength; i++)
            {
                T obj = factoryMethod.Invoke();
                objectPool.Push(obj);
            }
        }

        public T GetObject()
        {
            if (objectPool.Count == 0)
            {
                T newObj = factoryMethod.Invoke();
                return newObj;
            }
            else
            {
                return objectPool.Pop();
            }
        }

        public void ReturnObject(T obj)
        {
            if (objectPool.Count < poolLength)
            {
                objectPool.Push(obj);
            }
        }
    }
}