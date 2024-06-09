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
		private List<MaterialTarget> _destinations;
		public ImmutableList<MaterialTarget> Destinations => _destinations.ToImmutableList();
		public int DestinationCount => _destinations.Count;
		
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
			_destinations = new List<MaterialTarget>();
			_material = new Material(sourceMaterial);
		}
		
		public void AddRefForReplacement(MaterialTarget avatarMat)
		{
			_destinations.Add(avatarMat);
			_destName = null;
		}

		public void ApplyToDests(bool bypassCountCheck = false)
		{
			if (bypassCountCheck || DestinationCount > 1)
			{
				//Let the material be instead of replacing it
				var finalMaterial = Material;
				foreach (var dest in _destinations)
				{
					dest.SetNewMat(finalMaterial);
				}
			}
		}
	}
}