namespace TestData
{
  public class DuplicateMembers
  {
    public int Something
    {
      get => something;
      set => something = value;
    }

    private int something;
  }
}