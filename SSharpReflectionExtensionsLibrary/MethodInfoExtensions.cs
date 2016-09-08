using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp.Reflection
	{
	public static class MethodInfoExtensions
		{
		public static object CreateDelegate (this MethodInfo mi, Type delegateType)
			{
			return CDelegate.CreateDelegate (delegateType, null, mi);
			}

		public static object CreateDelegate (this MethodInfo mi, CType delegateType)
			{
			return CDelegate.CreateDelegate (delegateType, null, mi);
			}

		public static object CreateDelegate (this MethodInfo mi, Type delegateType, object target)
			{
			return CDelegate.CreateDelegate (delegateType, target, mi);
			}

		public static object CreateDelegate (this MethodInfo mi, CType delegateType, object target)
			{
			return CDelegate.CreateDelegate (delegateType, target, mi);
			}
		}
	}