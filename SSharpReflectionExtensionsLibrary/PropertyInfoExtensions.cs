using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp.Reflection
	{
	public static class PropertyInfoExtensions
		{
		public static void SetValue (this PropertyInfo pi, object obj, object value)
			{
			pi.SetValue (obj, value, null);
			}

		public static object GetValue (this PropertyInfo pi, object obj)
			{
			return pi.GetValue (obj, null);
			}
		}
	}