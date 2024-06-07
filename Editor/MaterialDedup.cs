using System;
using System.Collections.Generic;
using System.Linq;
using cc.dingemans.bigibas123.MaterialDedup;
using nadena.dev.ndmf;
using nadena.dev.ndmf.util;
using UnityEditor;
using UnityEngine;

[assembly: ExportsPlugin(typeof(MaterialDedup))]

namespace cc.dingemans.bigibas123.MaterialDedup
{
	public class MaterialDedup : Plugin<MaterialDedup>
	{
		public override string QualifiedName => "cc.dingemans.bigibas123.MaterialDedup.MaterialDedup";
		public override string DisplayName => "Material Deduplicator";

		private static readonly string TAG = "[MaterialDedup]";

		protected override void Configure()
		{
			InPhase(BuildPhase.Optimizing)
				.BeforePlugin("com.anatawa12.avatar-optimizer")
				.Run("Deduplicate materials", ctx =>
				{
					var avatar = ctx.AvatarRootTransform;
					var materials = CollectMaterials(avatar);
					var mappings = ResolveDeDups(materials);
					RunDeduplication(mappings);
				});
		}

		private List<MaterialReference> CollectMaterials(Transform avatar)
		{
			var materials = new List<MaterialReference>();
			foreach (var smr in avatar.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				var mats = smr.sharedMaterials;
				materials.AddRange(mats.Select((material, slot) => new MaterialReference(smr, slot)));
			}

			foreach (var mr in avatar.GetComponentsInChildren<MeshRenderer>())
			{
				var mats = mr.sharedMaterials;
				materials.AddRange(mats.Select((_, slot) => new MaterialReference(mr, slot)));
			}

			return materials;
		}

		private List<DeduplicatedMaterial> ResolveDeDups(List<MaterialReference> avatarMaterials)
		{
			var resolvedMaterials = new List<DeduplicatedMaterial>();
			foreach (var avatarMat in avatarMaterials)
			{
				if (avatarMat.Material == null) continue;

				var found = false;
				foreach (var resolved in resolvedMaterials)
				{
					if (TwoMaterialsAreSame(avatarMat.Material, resolved.Material) &&
					    MaterialsAreAnimatedTheSame(resolved, avatarMat))
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

		private bool MaterialsAreAnimatedTheSame(DeduplicatedMaterial resolved, MaterialReference avatarMat)
		{
			//TODO
			return true;
		}

		private bool TwoMaterialsAreSame(Material mat1, Material mat2)
		{
			if (mat1.shader != mat2.shader) return false;

			foreach (MaterialPropertyType propType in (MaterialPropertyType[])Enum.GetValues(
				         typeof(MaterialPropertyType)))
			{
				var mat1Props = mat1.GetPropertyNames(propType);
				var mat2Props = mat2.GetPropertyNames(propType);
				if (mat1Props.Length != mat2Props.Length) return false;
				if (mat1Props.Any(mat1Prop => !mat2Props.Contains(mat1Prop))) return false;

				foreach (var propName in mat1Props)
				{
					switch (propType)
					{
						case MaterialPropertyType.Float when Math.Abs(mat1.GetFloat(propName) - mat2.GetFloat(propName)) > Double.Epsilon:
							return false;
						case MaterialPropertyType.Int when mat1.GetInteger(propName) != mat2.GetInteger(propName):
							return false;
						case MaterialPropertyType.Vector when mat1.GetVector(propName) != mat2.GetVector(propName):
							return false;
						case MaterialPropertyType.Matrix when mat1.GetMatrix(propName) != mat2.GetMatrix(propName):
							return false;
						case MaterialPropertyType.Texture when mat1.GetTexture(propName) != mat2.GetTexture(propName):
							return false;
						case MaterialPropertyType.ConstantBuffer when mat1.GetConstantBuffer(propName) != mat2.GetConstantBuffer(propName):
							return false;
						case MaterialPropertyType.ComputeBuffer:
							Debug.LogError($"{TAG} Checking compute buffer equality is not supported right now!");
							return false;
					}
				}
				
			}

			return true;
		}

		private void RunDeduplication(List<DeduplicatedMaterial> mappings)
		{
			foreach (var dupMap in mappings)
			{
				dupMap.ApplyToDests();
			}
		}
	}
}