namespace BlazorWATemplate.Shared
{
    public class Enums
    {
        public enum UserRole
        {
            Unknown,
            Developer,
            OrderManager,
            ProductManager,
            ClientManager,
            DepartmentManager,
            SupplierManager,
            AccountingManager
        }

        public enum AccessType
        {
            Unknown,
            Edit,
            ReadOnly
        }

        public enum EmailTemplate
        {
            Unknown,
            AccountConfirmation,
            OrderConfirmation,
            PasswordRecovery,
            Contact
        }
    }
}
