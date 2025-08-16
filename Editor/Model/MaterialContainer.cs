using System;
using System.Linq;
using UnityEngine;

namespace cc.dingemans.bigibas123.materialdedup.Editor.Model
{
	public abstract class MaterialContainer
	{
		public abstract Material Material { get; }
		public abstract string Name { get; }

		public bool HasPropertiesSameAs(MaterialContainer other)
		{
			return HasPropertiesSameAs(this, other);
		}

		public static bool HasPropertiesSameAs(MaterialContainer matc1, MaterialContainer matc2)
		{
			var mat1 = matc1.Material;
			var mat2 = matc2.Material;
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
						case MaterialPropertyType.Float
							when Math.Abs(mat1.GetFloat(propName) - mat2.GetFloat(propName)) > Double.Epsilon:
							return false;
						case MaterialPropertyType.Int when mat1.GetInteger(propName) != mat2.GetInteger(propName):
							return false;
						case MaterialPropertyType.Vector when mat1.GetVector(propName) != mat2.GetVector(propName):
							return false;
						case MaterialPropertyType.Matrix when mat1.GetMatrix(propName) != mat2.GetMatrix(propName):
							return false;
						case MaterialPropertyType.Texture when mat1.GetTexture(propName) != mat2.GetTexture(propName):
							return false;
						case MaterialPropertyType.ConstantBuffer
							when mat1.GetConstantBuffer(propName) != mat2.GetConstantBuffer(propName):
							return false;
						case MaterialPropertyType.ComputeBuffer:
							Debug.LogError(
								$"{MaterialDedup.TAG} Checking compute buffer equality is not supported right now!");
							return false;
					}
				}
			}
			return true;
		}
		
	}
}