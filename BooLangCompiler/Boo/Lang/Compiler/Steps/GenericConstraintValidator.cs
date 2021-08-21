using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	public class GenericConstraintValidator
	{
		private readonly CompilerContext _context;

		private readonly GenericParameterDeclaration _gpd;

		private bool? _hasClassConstraint = null;

		private bool? _hasStructConstraint = null;

		private bool? _hasConstructorConstraint = null;

		private TypeReference _baseType = null;

		private EnvironmentProvision<TypeSystemServices> _typeSystemServices = default(EnvironmentProvision<TypeSystemServices>);

		protected CompilerContext Context => _context;

		protected bool HasClassConstraint
		{
			get
			{
				if (!_hasClassConstraint.HasValue)
				{
					_hasClassConstraint = HasConstraint(_gpd.Constraints, GenericParameterConstraints.ReferenceType);
				}
				return _hasClassConstraint.Value;
			}
		}

		protected bool HasStructConstraint
		{
			get
			{
				if (!_hasStructConstraint.HasValue)
				{
					_hasStructConstraint = HasConstraint(_gpd.Constraints, GenericParameterConstraints.ValueType);
				}
				return _hasStructConstraint.Value;
			}
		}

		protected bool HasConstructorConstraint
		{
			get
			{
				if (!_hasConstructorConstraint.HasValue)
				{
					_hasConstructorConstraint = HasConstraint(_gpd.Constraints, GenericParameterConstraints.Constructable);
				}
				return _hasConstructorConstraint.Value;
			}
		}

		protected TypeSystemServices TypeSystemServices => _typeSystemServices;

		public GenericConstraintValidator(CompilerContext context, GenericParameterDeclaration gpd)
		{
			_context = context;
			_gpd = gpd;
		}

		protected void Error(CompilerError error)
		{
			Context.Errors.Add(error);
		}

		public void Validate()
		{
			CheckAttributes();
			CheckTypeConstraints();
		}

		private void CheckAttributes()
		{
			if (HasClassConstraint && HasStructConstraint)
			{
				Error(CompilerErrorFactory.StructAndClassConstraintsConflict(_gpd));
			}
			if (HasStructConstraint && HasConstructorConstraint)
			{
				Error(CompilerErrorFactory.StructAndConstructorConstraintsConflict(_gpd));
			}
		}

		private void CheckTypeConstraints()
		{
			foreach (TypeReference baseType in _gpd.BaseTypes)
			{
				CheckTypeConstraint(baseType);
			}
		}

		private void CheckTypeConstraint(TypeReference tr)
		{
			IType type = (IType)tr.Entity;
			if (!IsValidTypeConstraint(type))
			{
				Error(CompilerErrorFactory.InvalidTypeConstraint(_gpd, tr));
			}
			if (type.IsClass)
			{
				if (_baseType == null)
				{
					_baseType = tr;
				}
				else
				{
					Error(CompilerErrorFactory.MultipleBaseTypeConstraints(_gpd, tr, _baseType));
				}
				if (HasStructConstraint)
				{
					Error(CompilerErrorFactory.TypeConstraintConflictsWithSpecialConstraint(_gpd, tr, "struct"));
				}
				if (HasClassConstraint)
				{
					Error(CompilerErrorFactory.TypeConstraintConflictsWithSpecialConstraint(_gpd, tr, "class"));
				}
			}
			if (type is IGenericParameter)
			{
			}
			if (!LessAccessibleThan(type, _gpd.ParentNode.Entity))
			{
			}
		}

		private bool IsValidTypeConstraint(IType type)
		{
			if (!type.IsInterface && !type.IsClass)
			{
				return false;
			}
			if (type.IsFinal)
			{
				return false;
			}
			if (type == TypeSystemServices.ArrayType || type == TypeSystemServices.ObjectType || type == TypeSystemServices.DelegateType || type == TypeSystemServices.ValueTypeType)
			{
				return false;
			}
			return true;
		}

		private bool LessAccessibleThan(IType left, IEntity right)
		{
			return false;
		}

		private bool HasConstraint(GenericParameterConstraints flags, GenericParameterConstraints flag)
		{
			return (flags & flag) == flag;
		}
	}
}
