using System.Collections.Generic;

namespace Framework.Selenium.Objects
{
    public abstract class BaseDictionary<T>
    {
        protected Dictionary<string, T> entryDictionary;

        protected BaseDictionary() { entryDictionary = new Dictionary<string, T>();}

        protected T Get(string name)
        {
            if (entryDictionary.TryGetValue(name, out var value))
            {
                return value;
            }
            //log
            throw new KeyNotFoundException();
        }

        protected void Add(string name, T item)
        {
            entryDictionary.Add(name, item);
        }

        public abstract void addElement(string name, T item);

        public abstract object getElement(string name);
    }
}
