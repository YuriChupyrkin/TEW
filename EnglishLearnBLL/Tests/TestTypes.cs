namespace EnglishLearnBLL.Tests
{
  internal interface ITestTypes
  {
    bool IsEnRu { get; }

    int MinLevel { get; }

    int MaxLevel { get; }
  }

  internal class EnRuPickerTest: ITestTypes
  {
    public bool IsEnRu
    {
      get
      {
        return true;
      }
    }

    public int MinLevel
    {
      get
      {
        return 0;
      }
    }

    public int MaxLevel
    {
      get
      {
        return 5;
      }
    }
  }
  internal class RuEnPickerTest : ITestTypes
  {
    public bool IsEnRu
    {
      get
      {
        return false;
      }
    }

    public int MinLevel
    {
      get
      {
        return 6;
      }
    }

    public int MaxLevel
    {
      get
      {
        return 10;
      }
    }
  }

  internal class WriteTest : ITestTypes
  {
    public bool IsEnRu
    {
      get
      {
        return true;
      }
    }

    public int MinLevel
    {
      get
      {
        return 11;
      }
    }

    public int MaxLevel
    {
      get
      {
        return 20;
      }
    }
  }
}
