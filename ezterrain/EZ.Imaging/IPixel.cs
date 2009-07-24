﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZ.Imaging
{
	public interface IPixel
	{
		void CopyFrom(byte[] data, int index);
		void CopyTo(byte[] data, int index);
	}
}
