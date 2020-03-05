using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using System.Runtime.InteropServices;
using Crestron.SimplSharp.Reflection;

namespace SSharp.Diagnostics
	{
	[SerializableAttribute]
	[ComVisibleAttribute (true)]
	public class StackFrame
		{
		private MethodBase _methodBase;

		public StackFrame ()
			{
			Frame = ReflectionUtilities.GetStackFrame (0);
			}

		public StackFrame (int skipFrames)
			{
			Frame = ReflectionUtilities.GetStackFrame (skipFrames);
			}

		internal StackFrame (string stackFrame)
			{
			Frame = stackFrame;
			}

		public MethodBase GetMethod ()
			{
			return _methodBase ?? (_methodBase = MethodBaseEx.GetMethodFromStackFrame (Frame));
			}

		internal string Frame { get; private set; }
		}
	}