using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.IO;
using EZ.Core;
using EZ.Imaging;

namespace EZ.Texturing
{
	public abstract class BoundTexture : Texture, IBound
	{
		protected BoundTexture(TextureUnit unit, Image image)
			: base(image)
		{
			Initialize(unit);
		}

		private void Initialize(TextureUnit unit)
		{
			this.Unit = unit;
			this.MagFilter = TextureMagFilter.Linear;
			this.MinFilter = TextureMinFilter.Nearest;
			this.WrapS = TextureWrapMode.ClampToEdge;
			this.WrapT = TextureWrapMode.ClampToEdge;
		}

		public abstract TextureTarget Target { get; }

		public TextureMagFilter MagFilter { get; set; }

		public TextureMinFilter MinFilter { get; set; }

		public TextureWrapMode WrapS { get; set; }

		public TextureWrapMode WrapT { get; set; }

		public TextureUnit Unit { get; set; }

		public int UnitIndex
		{
			get { return (int)this.Unit - (int)TextureUnit.Texture0; }
		}

		public int Handle { get; private set; }

		public override void Initialize()
		{
			if (!Initialized)
			{
				Handle = GL.GenTexture();

				Update();

				Initialized = true;
			}
		}

		private void SetParameters()
		{
			GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)MagFilter);
			GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)MinFilter);
			GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)WrapS);
			GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)WrapT);
		}

		public override void Update()
		{
			Bind();

			SetParameters();
		}

		#region Binding

		#region EnableCap
		protected virtual EnableCap? EnableCap { get { return null; } }

		private void Enable()
		{
			EnableCap? enableCap = this.EnableCap;
			if (enableCap.HasValue)
			{
				GL.Enable(enableCap.Value);
			}
		}

		private void Disable()
		{
			EnableCap? enableCap = this.EnableCap;
			if (enableCap.HasValue)
			{
				GL.Disable(enableCap.Value);
			}
		}
		#endregion

		public void Bind()
		{
			GL.ActiveTexture(Unit);
			Enable();
			GL.BindTexture(Target, Handle);
		}

		public void Unbind()
		{
			GL.ActiveTexture(Unit);
			GL.BindTexture(Target, 0);
			Disable();
		}
		#endregion

		#region Disposal
		protected override void Dispose(bool nongc)
		{
			if (nongc && Initialized)
			{
				GL.DeleteTexture(Handle);

				Initialized = false;
				Handle = 0;
			}

			base.Dispose(nongc);
		}
		#endregion
	}
}
