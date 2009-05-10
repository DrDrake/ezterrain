﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Objects
{
	public abstract class Disposable : IDisposable
	{
		public Disposable()
		{ }

		~Disposable()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public bool Disposed { get; private set; }

		protected virtual void Dispose(bool nongc)
		{
			Disposed = true;
		}
	}
}
