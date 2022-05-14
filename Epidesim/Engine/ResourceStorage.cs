using System;
using System.Collections;
using System.Collections.Generic;

namespace Epidesim.Engine
{
	class ResourceStorage<T> : IEnumerable<T>
	{
		private Dictionary<string, T> table = new Dictionary<string, T>();

		public void AddResource(string name, T item)
		{
			table.Add(name, item);
		}

		public T GetResource(string name)
		{
			return table[name];
		}

		public bool ResourceExists(string name)
		{
			return table.ContainsKey(name);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return table.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return table.Values.GetEnumerator();
		}
	}
}
