using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Machine.Specifications.Utility;

namespace Machine.Specifications.Model
{
  public class Context
  {
    readonly List<Specification> _specifications;
    readonly object _instance;
    readonly Subject _subject;
    readonly IEnumerable<Establish> _beforeEachs;
    readonly IEnumerable<Establish> _beforeAlls;
    readonly Because _because;
    readonly IEnumerable<Cleanup> _afterEachs;
    readonly IEnumerable<Cleanup> _afterAlls;
    public string Name { get; private set; }

    public object Instance
    {
      get { return _instance; }
    }

    public IEnumerable<Specification> Specifications
    {
      get { return _specifications; }
    }

    public Type Type { get; private set; }

    public Subject Subject
    {
      get { return _subject; }
    }

    public bool HasBecauseClause
    {
      get { return _because != null; }
    }

    public Context(Type type, object instance, IEnumerable<Establish> beforeEachs,
      IEnumerable<Establish> beforeAlls, Because because, IEnumerable<Cleanup> afterEachs,
      IEnumerable<Cleanup> afterAlls, Subject subject)
    {
      Name = type.Name.ReplaceUnderscores();
      Type = type;
      _instance = instance;
      _afterAlls = afterAlls;
      _afterEachs = afterEachs;
      _beforeAlls = beforeAlls;
      _because = because;
      _beforeEachs = beforeEachs;
      _specifications = new List<Specification>();
      _subject = subject;
      
    }

    public void AddSpecification(Specification specification)
    {
      _specifications.Add(specification);
    }

    public ContextVerificationResult Verify()
    {
      var verificationResults = VerifySpecifications();
      return new ContextVerificationResult(verificationResults);
    }

    IEnumerable<SpecificationVerificationResult> VerifySpecifications()
    {
      _beforeAlls.InvokeAll();
      var results = ExecuteSpecifications();
      _afterAlls.InvokeAll();

      return results;
    }

    IEnumerable<SpecificationVerificationResult> ExecuteSpecifications()
    {
      var results = new List<SpecificationVerificationResult>();
      foreach (Specification specification in _specifications)
      {
        var result = VerifySpecification(specification);
        results.Add(result);
      }

      return results;
    }

    public SpecificationVerificationResult VerifySpecification(Specification specification)
    {
      VerificationContext context = new VerificationContext(_instance);
      try
      {
        _beforeEachs.InvokeAll();
        _because.InvokeIfNotNull();
      }
      catch (Exception err)
      {
        return new SpecificationVerificationResult(err);
      }

      var result = specification.Verify(context);
      try
      {
        _afterEachs.InvokeAll();
      }
      catch (Exception err)
      {
        if (result.Passed)
        {
          return new SpecificationVerificationResult(err);
        }
        return result;
      }

      return result;
    }

    public SpecificationVerificationResult RunContextBeforeAll()
    {
      try
      {
        _beforeAlls.InvokeAll();
      }
      catch (Exception err)
      {
        return new SpecificationVerificationResult(err);
      }
      return null;
    }

    public SpecificationVerificationResult RunContextAfterAll()
    {
      try
      {
        _afterAlls.InvokeAll();
      }
      catch (Exception err)
      {
        return new SpecificationVerificationResult(err);
      }
      return null;
    }

    // TODO: Rename to Name
    public string FullName
    {
      get
      {
        string line = "";

        if (Subject != null)
        {
          line += Subject.FullConcern + ", ";
        }

        return line + Name;
      }
    }
  }
}