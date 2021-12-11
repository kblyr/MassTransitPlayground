namespace UserOrchestration.Entities;

record User
{
    public int Id { get; init; }
    public string Username { get; set; } = "";
    public string HashedPassword { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsPasswordChangeRequired { get; set; }
    public bool IsEmailVerified { get; set; }
}