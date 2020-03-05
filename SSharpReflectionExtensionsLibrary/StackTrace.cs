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
	public class StackTrace
		{
		private readonly string _stackTrace;

		public StackTrace ()
			{
			try
				{
				throw new Exception ();
				}
			catch (Exception ex)
				{
				_stackTrace = ex.StackTrace;
				}
			}

		public StackTrace (StackFrame frame)
			{
			_stackTrace = frame.Frame;
			}

		public StackTrace (Exception e)
			{
			_stackTrace = e.StackTrace;
			}

		public StackTrace (int skipFrames)
			{
			_stackTrace = ReflectionUtilities.GetStackFrame (skipFrames);
			}

		public StackTrace (Exception e, int skipFrames)
			{
			_stackTrace = ReflectionUtilities.GetStackFrame (e.StackTrace, skipFrames);
			}

		public StackFrame GetFrame (int index)
			{
			return new StackFrame(ReflectionUtilities.GetStackFrame (_stackTrace, index));
			}

		public StackFrame[] GetFrames ()
			{
			return _stackTrace.Split ('\n').Select (f => new StackFrame (f)).ToArray ();
			}

		public virtual int FrameCount
			{
			get
				{
				return _stackTrace.Count (c => c == '\n');
				}
			}
		}
	}