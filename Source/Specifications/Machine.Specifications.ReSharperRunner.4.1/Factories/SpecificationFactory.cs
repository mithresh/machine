using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.UnitTestExplorer;

using Machine.Specifications.ReSharperRunner.Presentation;

namespace Machine.Specifications.ReSharperRunner.Factories
{
  internal class SpecificationFactory
  {
    readonly IProjectModelElement _project;
    readonly IUnitTestProvider _provider;

    public SpecificationFactory(IUnitTestProvider provider, IProjectModelElement project)
    {
      _provider = provider;
      _project = project;
    }

    public SpecificationElement CreateSpecificationElement(IDeclaredElement field)
    {
      IClass clazz = field.GetContainingType() as IClass;
      if (clazz == null)
      {
        return null;
      }

      ContextElement context = ContextCache.Classes[clazz];
      if (context == null)
      {
        return null;
      }

      return new SpecificationElement(_provider,
                                      context,
                                      _project,
                                      clazz.CLRName,
                                      field.ShortName,
                                      field.IsIgnored());
    }

    public SpecificationElement CreateSpecificationElement(ContextElement context, IMetadataField specification)
    {
      return new SpecificationElement(_provider,
                                      context,
                                      _project,
                                      specification.DeclaringType.FullyQualifiedName,
                                      specification.Name,
                                      specification.IsIgnored());
    }
  }
}