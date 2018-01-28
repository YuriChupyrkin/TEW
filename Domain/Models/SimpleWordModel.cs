using System;

namespace Domain.RepositoryFactories.Models
{
  public class SimpleWordModel
  {
    public int Id { get; set; }

    public string English { get; set; }

    public string Russian { get; set; }

    public string Example { get; set; }

    public int UserId { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int Level { get; set; }

    public int AnswerCount { get; set; }

    public int FailAnswerCount { get; set; }

  }
}
