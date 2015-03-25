using System.Linq;
using System;
// For Basic SIMPL# Classes

namespace Crestron.SimplSharp.Reflection
	{
	public static class CTypeExtensions
		{
		public static CType GetCType (this object obj)
			{
			return obj.GetType ();
			}

		public static CType ctypeof<T> ()
			{
			return (typeof (T));
			}

		public static CType[] GetTypeArray (object[] args)
			{
			if (args == null)
				return CTypeEmptyArray;

			return args.Select (a => a == null ? (CType)typeof(object) : (a is Type ? (CType)typeof(CType) : a.GetCType ())).ToArray ();
			}

		public static readonly CType[] CTypeEmptyArray = new CType[0];
		}
	}
