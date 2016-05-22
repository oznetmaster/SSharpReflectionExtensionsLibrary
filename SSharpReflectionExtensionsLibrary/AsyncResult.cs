using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Crestron.SimplSharp.CrestronIO
	{
	public class AsyncResult : IAsyncResult
		{
		private readonly CEvent _cevent = new CEvent (false, false);

		#region IAsyncResult Members

		public virtual object AsyncState
			{
			get;
			private set;
			}

		public virtual bool CompletedSynchronously
			{
			get;
			internal set;
			}

		public virtual bool IsCompleted
			{
			get;
			internal set;
			}

		public virtual CEventHandle AsyncWaitHandle
			{
			get { return _cevent; }
			}

		public object InnerObject
			{
			get { throw new NotImplementedException (); }
			}

		#endregion

		public bool EndInvokeCalled
			{
			get;
			set;
			}

		public virtual Delegate AsyncDelegate
			{
			get;
			private set;
			}

		internal object Result
			{
			get;
			set;
			}

		internal Exception Exception
			{
			get;
			set;
			}

		internal AsyncResult (Delegate asyncDelegate, object obj)
			{
			AsyncDelegate = asyncDelegate;
			AsyncState = obj;
			}
		}
	}