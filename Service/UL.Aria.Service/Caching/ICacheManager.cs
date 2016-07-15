using System;

namespace UL.Aria.Service.Caching
{
	/// <summary>
	/// Provides an interface for a Cache Manager
	/// </summary>
	public interface ICacheManager
	{
		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		T GetItem<T>(string key);

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		object GetItem(string key);

		/// <summary>
		/// Stores the item.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="data">The data.</param>
		/// <param name="expiry">The expiry.</param>
		void StoreItem(string key, object data, TimeSpan expiry = default(TimeSpan));

		/// <summary>
		/// Gets the grouped item.
		/// </summary>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		object GetGroupedItem(string groupId, string key);

		/// <summary>
		/// Stores the grouped item.
		/// </summary>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		/// <param name="data">The data.</param>
		void StoreGroupedItem(string groupId, string key, object data);

		/// <summary>
		/// Stores the index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="key">The key.</param>
		void StoreIndex(string index, string key);

		/// <summary>
		/// Gets the index key.
		/// </summary>
		/// <param name="index">The index.</param>
		string GetIndex(string index);

		/// <summary>
		/// Removes the index.
		/// </summary>
		/// <param name="key">The key.</param>
		void RemoveIndex(string key);

		/// <summary>
		/// Removes the item.
		/// </summary>
		/// <param name="key">The key.</param>
		void RemoveItem(string key);

		/// <summary>
		/// Gets the grouped item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		T GetGroupedItem<T>(string groupId, string key);

		/// <summary>
		/// Removes the grouped item.
		/// </summary>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		void RemoveGroupedItem(string groupId, string key);
	}
}