using System;
using System.Security.Permissions;

namespace Boo.Lang.Compiler.Util
{
	internal static class Permissions
	{
		private static bool? hasAppDomainPermission;

		private static bool? hasEnvironmentPermission;

		private static bool? hasDiscoveryPermission;

		public static T WithEnvironmentPermission<T>(Func<T> function)
		{
			return WithPermission(ref hasEnvironmentPermission, () => new EnvironmentPermission(PermissionState.Unrestricted), function);
		}

		public static T WithDiscoveryPermission<T>(Func<T> function)
		{
			return WithPermission(ref hasDiscoveryPermission, () => new FileIOPermission(PermissionState.Unrestricted), function);
		}

		public static void WithAppDomainPermission(Action action)
		{
			WithPermission(ref hasAppDomainPermission, () => new SecurityPermission(SecurityPermissionFlag.ControlAppDomain), delegate
			{
				action();
				return false;
			});
		}

		private static TRetVal WithPermission<TPermission, TRetVal>(ref bool? hasPermission, Func<TPermission> permission, Func<TRetVal> function)
		{
			if (hasPermission.HasValue && !hasPermission.Value)
			{
				return default(TRetVal);
			}
			try
			{
				permission();
				return function();
			}
			catch (Exception)
			{
				hasPermission = false;
				return default(TRetVal);
			}
		}
	}
}
