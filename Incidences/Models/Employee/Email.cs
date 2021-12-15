namespace Incidences.Models.Employee
{
    public class Email
    {
        public string mailName { get; }
        public string domain { get; }
        public string fullEmail { get; }
        public Email(string mailName, string domain)
        {
            this.mailName = mailName;
            this.domain = domain;
            this.fullEmail = $"{mailName}@{domain}";
        }
        public Email(Data.Models.Email email)
        {
            this.mailName = email.name;
            this.domain = email.domain;
            this.fullEmail = $"{email.name}@{email.domain}";
        }
    }
}
