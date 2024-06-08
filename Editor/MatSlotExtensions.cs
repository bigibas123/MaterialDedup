using System;
using System.Collections.Generic;
using System.Linq;
using cc.dingemans.bigibas123.MaterialDedup.Editor.Model;
using cc.dingemans.bigibas123.MaterialDedup.Runtime;
using UnityEngine;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor
{
	public static class MatSlotExtensions
	{
		public static IEnumerable<MaterialReference> AsMaterialRefs(this MaterialDeduplicatorBehavior dup)
		{
			//TODO add materials referenced in animations as well
			return dup.Renderers.AsMaterialRefs();
		}

		public static IEnumerable<MaterialReference> AsMaterialRefs(this IEnumerable<Renderer> renders) =>
			renders.SelectMany(renderer => renderer.AsMaterialRefs());

		public static IEnumerable<MaterialReference> AsMaterialRefs(this Renderer rend) =>
			rend.sharedMaterials.Select((_, b) => new MaterialReference(rend, b));

		public static List<DeduplicatedMaterial> AsDedupList(this IEnumerable<MaterialReference> avatarMaterials)
		{
			var resolvedMaterials = new List<DeduplicatedMaterial>();
			foreach (var avatarMat in avatarMaterials)
			{
				if (avatarMat.Material == null) continue;

				var found = false;
				foreach (var resolved in resolvedMaterials)
				{
					if (
						avatarMat.HasPropertiesSameAs(resolved)
						&& IsAnimatedTheSame(resolved,avatarMat)
					)
					{
						found = true;
						resolved.AddRefForReplacement(avatarMat);
						break;
					}
				}

				if (found) continue;
				var newMaterial = new DeduplicatedMaterial(avatarMat.Material);
				newMaterial.AddRefForReplacement(avatarMat);
				resolvedMaterials.Add(newMaterial);
			}

			return resolvedMaterials;
		}

		private static bool IsAnimatedTheSame(DeduplicatedMaterial dedupped, MaterialReference mat)
		{
			return true; //TODO
		}
	}
}