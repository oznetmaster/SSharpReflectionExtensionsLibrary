#region License
/*
 * AsyncResult.cs
 *
 * The MIT License
 *
 * Copyright © 2017 Nivloc Enterprises Ltd
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

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