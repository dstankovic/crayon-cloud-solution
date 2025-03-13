using CloudSales.Domain.Common;
using CloudSales.Domain.Enums;

namespace CloudSales.Domain.Entities;

public class User : BaseEntity
{
    private User() { }

    public User(string username, string email, string firstName, string lastName, UserType type, Customer customer)
    {
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Type = type;
        Customer = customer;
    }

    public string Username { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserType Type { get; private set; }

    public virtual Customer Customer { get; private set; }
}
