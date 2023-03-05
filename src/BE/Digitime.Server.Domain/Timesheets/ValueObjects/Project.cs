namespace Digitime.Server.Domain.Timesheets.ValueObjects;

public class Project
{
    public string Id { get; }
    public string Title { get; }
    public string Code { get; }

    public Project(string id, string title, string code)
    {
        Id = id;
        Title = title;
        Code = code;
    }
}