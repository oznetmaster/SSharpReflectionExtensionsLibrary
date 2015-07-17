using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using System.Globalization;

namespace Crestron.SimplSharp.Reflection
	{
	public static class ActivatorEx
		{
		private const BindingFlags BfNonPublic = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		private const BindingFlags BfPublic = BindingFlags.Public | BindingFlags.Instance;

		public static object CreateInstance (Type type)
			{
			return Activator.CreateInstance (type);
			}

		public static T CreateInstance<T> () where T : new()
			{
			return Activator.CreateInstance<T> ();
			}

		public static object CreateInstance (Type type, bool nonPublic)
			{
			CType ct = type;

			if (!nonPublic)
				return Activator.CreateInstance (ct);

			var ci = ct.GetConstructor (BfNonPublic, null, CTypeExtensions.CTypeEmptyArray, null);
			if (ci == null)
				throw new MissingMethodException ();

			return ci.Invoke (null);
			}

		public static object CreateInstance (Type type, params object[] args)
			{
			CType ct = type;

			if (args == null || args.Length == 0)
				return Activator.CreateInstance (ct);

			var ci = ct.GetConstructor (CTypeExtensions.GetTypeArray (args));
			if (ci == null)
				throw new MissingMethodException ();

			return ci.Invoke (args);
			}

		public static object CreateInstance (Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
			{
			var ci = type.GetCType ().GetConstructor (bindingAttr, binder, CTypeExtensions.GetTypeArray (args), null);
			if (ci == null)
				throw new MissingMethodException ();

			return ci.Invoke (bindingAttr, binder, args, culture);
			}
		}
	}