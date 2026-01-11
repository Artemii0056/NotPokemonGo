using System;
using VContainer;

namespace Infrastructure.DI.DIExtensions
{
	public static partial class ContainerBuilderExtensions
	{
		public static IContainerBuilder RegisterIf(
			this IContainerBuilder builder, 
			bool condition, 
			Func<IContainerBuilder> trueCondition,
			Func<IContainerBuilder> falseCondition)
		{
			if (condition) 
				trueCondition();
			else
				falseCondition();
			
			return builder;
		}
	}
}