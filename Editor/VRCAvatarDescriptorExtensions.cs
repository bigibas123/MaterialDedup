﻿using System.Collections.Generic;
using System.Linq;
using cc.dingemans.bigibas123.MaterialDedup.Editor.Model;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Animation
{
	public static class VRCAvatarDescriptorExtensions
	{
		public static IEnumerable<MaterialTarget> GetMaterialTargetFromAnimationLayers(
			this VRCAvatarDescriptor descriptor)
		{
			if (descriptor)
			{
				return descriptor.baseAnimationLayers.GetAnimationControllers()
					.Concat(descriptor.specialAnimationLayers.GetAnimationControllers())
					.SelectMany(cont => cont.GetAllMaterialRefrences());
			}

			return new List<MaterialTarget>(0);
		}

		public static IEnumerable<MaterialTarget> GetAllMaterialRefrences(this RuntimeAnimatorController cont)
		{
			if (cont)
			{
				return cont.animationClips
					.SelectMany(clip => AnimationUtility.GetObjectReferenceCurveBindings(clip)
						.Where(binding => binding.type.IsTargetTypeSupported())
						.Where(binding => binding.propertyName.StartsWith("m_Materials.Array"))
						.SelectMany(binding => binding.GetMaterialsFromBinding(clip)
						)
					);
			}

			return new List<MaterialTarget>(0);

			;
		}

		public static IEnumerable<MaterialTarget> GetMaterialsFromBinding(this EditorCurveBinding binding,
			AnimationClip clip)
		{
			return AnimationUtility.GetObjectReferenceCurve(clip, binding)
				.Select(
					(_, i) => new AnimationMaterialTarget(clip, binding, i)
				);
		}

		public static IEnumerable<RuntimeAnimatorController> GetAnimationControllers(
			this VRCAvatarDescriptor.CustomAnimLayer[] layers)
		{
			var controllers = new List<RuntimeAnimatorController>();
			for (var i = 0; i < layers.Length; i++)
			{
				var layer = layers[i];
				if (!layer.isDefault)
				{
					controllers.Add(layer.animatorController);
				}
			}

			return controllers;
		}
	}
}