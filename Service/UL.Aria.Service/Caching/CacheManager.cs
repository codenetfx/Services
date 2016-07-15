using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using StackExchange.Redis;

using UL.Aria.Service.Configuration;

namespace UL.Aria.Service.Caching
{
	/// <summary>
	/// The Redis Cache Manager
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class CacheManager : ICacheManager
	{
		private readonly bool _isRedisCacheEnabled;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly ConnectionMultiplexer _redisConnector;
		// ReSharper disable once InconsistentNaming
		private readonly IDatabase _redisDB;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly IServiceConfiguration _serviceConfig;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheManager"/> class.
		/// </summary>
		public CacheManager(IServiceConfiguration serviceConfig)
		{
			_serviceConfig = serviceConfig;

			_isRedisCacheEnabled = _serviceConfig.IsRedisCacheEnabled;

			if (_isRedisCacheEnabled)
			{
				_redisConnector = ConnectionMultiplexer.Connect(_serviceConfig.RedisCacheConnectionString);
				_redisDB = _redisConnector.GetDatabase();
			}
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public T GetItem<T>(string key)
		{
			return (T) GetItem(key);
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public object GetItem(string key)
		{
			if (_isRedisCacheEnabled)
			{
				var userobj = _redisDB.StringGet(key);
				return Deserialize(userobj);
			}
			return null;
		}

		/// <summary>
		/// Stores the item.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="data">The data.</param>
		/// <param name="expiry">The expiry.</param>
		public void StoreItem(string key, object data, TimeSpan expiry = default(TimeSpan))
		{
			if (_isRedisCacheEnabled)
			{
				if (expiry == default(TimeSpan)) expiry = new TimeSpan(1, 0, 0, 0, 0);
				_redisDB.StringSet(key, SerializeObject(data), expiry, When.Always, CommandFlags.DemandMaster);
			}
		}

		/// <summary>
		/// Removes the item.
		/// </summary>
		/// <param name="key">The key.</param>
		public void RemoveItem(string key)
		{
			if (_isRedisCacheEnabled)
			{
				_redisDB.KeyDelete(key);
			}
		}

		/// <summary>
		/// Gets the grouped item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public T GetGroupedItem<T>(string groupId, string key)
		{
			return (T) GetGroupedItem(groupId, key);
		}

		/// <summary>
		/// Gets the grouped item.
		/// </summary>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public object GetGroupedItem(string groupId, string key)
		{
			if (_isRedisCacheEnabled)
			{
				var obj = _redisDB.HashGet(groupId, key, CommandFlags.DemandMaster);
				return Deserialize(obj);
			}
			return null;
		}

		/// <summary>
		/// Stores the grouped item.
		/// </summary>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		/// <param name="data">The data.</param>
		public void StoreGroupedItem(string groupId, string key, object data)
		{
			if (_isRedisCacheEnabled)
			{
				var objBytes = SerializeObject(data);
				_redisDB.HashSet(groupId, key, objBytes, When.Always, CommandFlags.DemandMaster);
			}
		}

		/// <summary>
		/// Removes the grouped item.
		/// </summary>
		/// <param name="groupId">The group identifier.</param>
		/// <param name="key">The key.</param>
		/// <exception cref="NotImplementedException"></exception>
		public void RemoveGroupedItem(string groupId, string key)
		{
			if (_isRedisCacheEnabled)
			{
				_redisDB.HashDelete(groupId, key);
			}
		}

		/// <summary>
		/// Stores the index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="key">The key.</param>
		public void StoreIndex(string index, string key)
		{
			if (_isRedisCacheEnabled)
			{
				var hashId = GetHashID(index);
				_redisDB.HashSet(hashId, index, key, When.Always, CommandFlags.DemandMaster);
			}
		}

		/// <summary>
		/// Gets the index key.
		/// </summary>
		/// <param name="index">The index.</param>
		public string GetIndex(string index)
		{
			if (_isRedisCacheEnabled)
			{
				var hashId = GetHashID(index);
				return _redisDB.HashGet(hashId, index, CommandFlags.DemandMaster);
			}
			return null;
		}

		/// <summary>
		/// Removes the index.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void RemoveIndex(string key)
		{
			if (_isRedisCacheEnabled)
			{
				var hashId = GetHashID(key);
				_redisDB.HashDelete(hashId, key);
			}
		}

		/// <summary>
		/// Gets a hash identifier based on the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private string GetHashID(string index)
		{
			//to control the number of groups and the avg num items in a hash group
			//the relationshiup between the divisor and the number if expected items to be hashed
			//is a polynomial equations that would need to be determined but for now we are using a constant.
			//reduce the constant to create more groups.
			const int groupDivisor = 1000000;
			var hash = index.GetHashCode();
			// ReSharper disable once PossibleLossOfFraction
			return (Math.Abs((int) Math.Floor((decimal) (hash/groupDivisor)))).ToString();
		}

		private byte[] SerializeObject(object obj)
		{
			byte[] data;

			using (var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, obj);
				stream.Position = 0;
				data = new byte[stream.Length];
				stream.Read(data, 0, (int) stream.Length);
			}

			return data;
		}

		private object Deserialize(byte[] data)
		{
			if (data == null || data.Length <= 0) return null;

			using (var stream = new MemoryStream())
			{
				stream.Write(data, 0, data.Length);
				stream.Position = 0;
				var formatter = new BinaryFormatter();
				return formatter.Deserialize(stream);
			}
		}
	}
}