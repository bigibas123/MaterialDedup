using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Model
{
	public class DeduplicatedMaterial : MaterialContainer
	{
		private static readonly string _prefix = "Dedup:";

		private static readonly (string find, string replace)[] _subStringReplacements =
			{ ("VRCFury Material for ", "VRCF") };

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
				if (_destName == null)
				{
					IEnumerable<string> matNames = _destinations
							.Select(matRef => matRef.Name)
							.Select(name =>
								_subStringReplacements
									.Aggregate(name,
										(current, tup) =>
											current
												.Replace(tup.find, tup.replace)
									)
							)
						;
					_destName = $"{_prefix}{string.Join(";", matNames)}";
				}

				return _destName;
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