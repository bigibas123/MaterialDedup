using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace cc.dingemans.bigibas123.materialdedup.Editor.Model
{
	public class RendererMaterialReference : MaterialTarget
	{
		public override Material Material => (Renderer != null) ? Renderer.sharedMaterials[Slot] : null;

		public override string Name
		{
			get
			{
				var s = $"{Renderer.gameObject.name}[{Slot}";
				if (Material != null)
				{
					s += $"->{Material.name}";
				}
				return s + "]";
			}
		}

		public int Slot { get; }

		public RendererMaterialReference(Renderer renderer, int slot)
		{
			Renderer = renderer;
			Slot = slot;
		}

		private TargetType _type;

		public override TargetType Type
		{
			get => _type;
		}

		public override Object Target => Renderer;

		private Renderer _renderer;

		private Renderer Renderer
		{
			get => _renderer;
			set
			{
				_renderer = value;
				_type = value.GetType().GetTargetTypeFrom();
			}
		}

		public override void SetNewMat(Material mat)
		{
			switch (Type)
			{
				case TargetType.Renderer:
					var mats = Renderer.sharedMaterials;
					mats[Slot] = mat;
					Renderer.sharedMaterials = mats;
					break;
				default:
					throw new ArgumentOutOfRangeException("type", Type, "Type not implemented");
			}
		}
	}
}