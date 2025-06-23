namespace AzureFunctions.Users
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public DateTime created_at { get; set; }

        public User() { }

        public User(int Id)
        {
            id = Id;
        }


         public override string ToString()
         {
             return $"Id: {id}, Name: {username}, Email: {email}"; // Formatação
         }
    }
}