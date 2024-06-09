using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Model
{
	public class RendererMaterialReference : MaterialTarget
	{
		public override Material Material => (Renderer != null && Slot.HasValue) ? Renderer.sharedMaterials[Slot.Value] : null;
		public override string Name => $"{(Material != null ? Material.name : $"{Renderer.gameObject.name}[{Slot}]")}";
		
		private int? Slot { get; }

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
		

		[CanBeNull]
		private Renderer Renderer
		{
			get => _renderer;
			set
			{
				_renderer = value;
				_type = value switch
				{
					SkinnedMeshRenderer => TargetType.Skinned,
					MeshRenderer => TargetType.Static,
					_ => throw new InvalidParameterException(
						"Did not provide skinned or static Mesh Renderer to MaterialReference: " + value)
				};
			}
		}

		public override void SetNewMat(Material mat)
		{
			switch (Type)
			{
				case TargetType.Static:
				case TargetType.Skinned:
					var mats = Renderer.sharedMaterials;
					mats[Slot.Value] = mat;
					Renderer.sharedMaterials = mats;
					break;
				default:
					throw new ArgumentOutOfRangeException("type",Type,"Type not implemented");
			}
		}
	}
}