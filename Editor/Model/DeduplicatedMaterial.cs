using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Model
{
	public class DeduplicatedMaterial : MaterialContainer
	{
		private string _prefix = "Dedup:";
		private List<MaterialReference> _destinations;
		public ImmutableList<MaterialReference> Destinations => _destinations.ToImmutableList();
		private Material _material;
		[CanBeNull] private string _destName;

		public override Material Material
		{
			get
			{
				_material.name = $"{Name}";
				return _material;
			}
		}

		public override string Name
		{
			get
			{
				return _destName ??= $"{_prefix}{string.Join(";", _destinations.Select((matRef) => matRef.Name))}";
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