using ExpensesReport.Users.Core.Entities;

namespace ExpensesReport.Expenses.Core.Entities
{
    public class Signature : EntityBase
    {
        public Signature(string name, bool acceptance, DateTime signatureDate, string ipAddress)
        {
            Name = name;
            Acceptance = acceptance;
            SignatureDate = signatureDate;
            IpAddress = ipAddress;
        }

        public string Name { get; set; }
        public bool Acceptance { get; set; }
        public DateTime SignatureDate { get; set; }
        public string IpAddress { get; set; }
    }
}
