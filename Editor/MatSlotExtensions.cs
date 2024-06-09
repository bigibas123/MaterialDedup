using System;
using System.Collections.Generic;
using System.Linq;
using cc.dingemans.bigibas123.MaterialDedup.Editor.Animation;
using cc.dingemans.bigibas123.MaterialDedup.Editor.Model;
using cc.dingemans.bigibas123.MaterialDedup.Runtime;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor
{
	public static class MatSlotExtensions
	{
		public static IEnumerable<MaterialTarget> AsMaterialRefs(this MaterialDeduplicatorBehavior dup,
			VRCAvatarDescriptor contextAvatarDescriptor)
		{
			IEnumerable<MaterialTarget> rendererMaterials = dup.Renderers.AsMaterialRefs();
			IEnumerable<MaterialTarget> animationLayerMats =
				contextAvatarDescriptor.GetMaterialTargetFromAnimationLayers();
			return rendererMaterials.Concat(animationLayerMats);
		}

		public static IEnumerable<RendererMaterialReference> AsMaterialRefs(this IEnumerable<Renderer> renders) =>
			renders.SelectMany(renderer => renderer.AsMaterialRefs());

		public static IEnumerable<RendererMaterialReference> AsMaterialRefs(this Renderer rend) =>
			rend.sharedMaterials.Select((_, b) => new RendererMaterialReference(rend, b));

		public static List<DeduplicatedMaterial> AsDedupList(this IEnumerable<MaterialTarget> avatarMaterials)
		{
			var resolvedMaterials = new List<DeduplicatedMaterial>();
			foreach (var avatarMat in avatarMaterials)
			{
				if (avatarMat.Material == null) continue;

				var found = false;
				foreach (var resolved in resolvedMaterials)
				{
					if (!avatarMat.HasPropertiesSameAs(resolved)) continue;
					found = true;
					resolved.AddRefForReplacement(avatarMat);
					break;
				}

				if (found) continue;
				var newMaterial = new DeduplicatedMaterial(avatarMat.Material);
				newMaterial.AddRefForReplacement(avatarMat);
				resolvedMaterials.Add(newMaterial);
			}

			return resolvedMaterials;
		}
	}
}