namespace datingapp.api.Models
{
    public class user
    {
        public int id { get; set; }
        public string username { get; set; }
        public byte[] passwordhash { get; set; }
        public byte[] passwordsalt { get; set; }
    }
}