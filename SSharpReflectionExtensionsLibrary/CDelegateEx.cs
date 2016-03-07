using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;

namespace Crestron.SimplSharp.Reflection
	{
	public class CDelegateEx
		{
		public static TDelegate CreateDelegate<TDelegate> (MethodInfo method) where TDelegate : class
			{
			return CDelegate.CreateDelegate (typeof (TDelegate), null, method) as TDelegate;
			}

		public static TDelegate CreateDelegate<TDelegate> (object firstArgument, MethodInfo method) where TDelegate : class
			{
			return CDelegate.CreateDelegate (typeof (TDelegate), firstArgument, method) as TDelegate;
			}

		public static TDelegate CreateDelegate<TDelegate> (object target, string method) where TDelegate : class
			{
			return CDelegate.CreateDelegate (typeof (TDelegate), target, target.GetCType ().GetMethod (method)) as TDelegate;
			}

		public static TDelegate CreateDelegate<TDelegate> (Type target, string method) where TDelegate : class
			{
			return CDelegate.CreateDelegate (typeof (TDelegate), null, target.GetCType ().GetMethod (method)) as TDelegate;
			}

		public static TDelegate CreateDelegate<TDelegate> (CType target, string method) where TDelegate : class
			{
			return CDelegate.CreateDelegate (typeof (TDelegate), null, target.GetMethod (method)) as TDelegate;
			}

		public static TDelegate CreateDelegate<TDelegate, TTarget> (string method) where TDelegate : class
			{
			return CDelegate.CreateDelegate (typeof (TDelegate), null, typeof(TTarget).GetCType ().GetMethod (method)) as TDelegate;
			}
		}
	}