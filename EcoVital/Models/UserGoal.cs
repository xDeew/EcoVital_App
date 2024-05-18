namespace EcoVital.Models;

public class UserGoal
{
    public int GoalId { get; set; }
    public DateTime TargetDate { get; set; }
    public bool IsAchieved { get; set; }
    public int UserId { get; set; }

    public int ActivityRecordId { get; set; }
}