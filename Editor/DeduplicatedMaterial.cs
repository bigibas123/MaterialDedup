using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace cc.dingemans.bigibas123.MaterialDedup
{
	public class DeduplicatedMaterial
	{
		private string _prefix = "Dedup_";
		private List<MaterialReference> _destinations;
		private Material _original;
		private Material _material;
		[CanBeNull] private string _destName;

		public Material Material
		{
			get
			{
				_material.name = $"{Name}";
				return _material;
			}
		}

		public string Name
		{
			get
			{
				return _destName ??= $"{_prefix}{_original.name}:{string.Join("_", _destinations.Select((matRef) => matRef.DestinationName))}";
			}
		}
		
		public DeduplicatedMaterial(Material sourceMaterial)
		{
			_original = sourceMaterial;
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
			if (_destinations.Count <= 1)
			{
				//Let the material be instead of replacing it
				return;
			}
			var finalMaterial = Material;
			foreach (var dest in _destinations)
			{
				dest.SetNewMat(finalMaterial);
			}
		}
	}
}