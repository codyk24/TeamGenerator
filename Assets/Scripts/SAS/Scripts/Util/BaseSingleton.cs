using System;

namespace SAS
{
	/// <summary>
	/// Abstract singleton class that exists externally from any Unity assets / behaviours.
	/// Concrete subclasses must have a private parameterless constructor.
	/// </summary>
	/// <typeparam name="T">Singleton class</typeparam>
	public abstract class BaseSingleton<T>
		where T : BaseSingleton<T>
	{
		#region Fields

		private static BaseSingleton<T> s_instance;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the instance of the singleton
		/// </summary>
		public static BaseSingleton<T> Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = Activator.CreateInstance(typeof(T), true) as T;
				}
				return s_instance;
			}
		}

		/// <summary>
		/// Gets the cast instance of the singleton
		/// </summary>
		public static T CastInstance => Instance as T;

		#endregion
	}
}
