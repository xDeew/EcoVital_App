namespace EcoVital.Models;

public class SecurityQuestion
{
    public int SecurityQuestionId { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }
    public int UserId { get; set; }
}