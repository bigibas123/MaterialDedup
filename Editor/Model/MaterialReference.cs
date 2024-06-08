using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using JetBrains.Annotations;
using UnityEngine;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Model
{
	public class MaterialReference : MaterialContainer
	{
		public int Slot { get; }

		public override Material Material => Renderer.sharedMaterials[Slot];
		public override string Name => $"{Renderer.gameObject.name}[{Slot}]";
		[CanBeNull] public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
		[CanBeNull] public MeshRenderer MeshRenderer { get; private set; }
		
		public MaterialReference(Renderer renderer, int slot)
		{
			Renderer = renderer;
			Slot = slot;
		}

		public Renderer Renderer
		{
			get => SkinnedMeshRenderer != null ? SkinnedMeshRenderer : MeshRenderer;
			private set
			{
				switch (value)
				{
					case SkinnedMeshRenderer smr:
						SkinnedMeshRenderer = smr;
						break;
					case MeshRenderer mr:
						MeshRenderer = mr;
						break;
					default:
						throw new InvalidParameterException(
							"Did not provide skinned or static Mesh Renderer to MaterialReference: " + value);
				}
			}
		}

		public void SetNewMat(Material mat)
		{
			var mats = Renderer.sharedMaterials;
			mats[Slot] = mat;
			Renderer.sharedMaterials = mats;
		}
	}
}