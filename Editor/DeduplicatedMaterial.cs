using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace cc.dingemans.bigibas123.MaterialDedup
{
	public class DeduplicatedMaterial
	{
		private string _prefix = "Deduplicated_material_for:";
		private List<MaterialReference> _destinations;
		private Material _material;
		[CanBeNull] private string _destName;

		public Material Material
		{
			get
			{
				_material.name = $"{_prefix}{DestName}";
				return _material;
			}
		}

		public string DestName
		{
			get
			{
				return _destName ??= string.Join("_", _destinations.Select((matRef) => matRef.DestinationName));
			}
		}
		
		public DeduplicatedMaterial(Material sourceMaterial)
		{
			_destinations = new List<MaterialReference>();
			_material = new Material(sourceMaterial);
		}
		
		public void AddRefForReplacement(MaterialReference avatarMat)
		{
			_destinations.Add(avatarMat);
			_destName = null;
		}

		public void ApplyToDests()
		{
			var finalMaterial = Material;
			foreach (var dest in _destinations)
			{
				dest.SetNewMat(finalMaterial);
			}
		}
	}
}