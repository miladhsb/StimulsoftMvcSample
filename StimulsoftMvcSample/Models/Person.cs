namespace StimulsoftMvcSample.Models
{
    public class Person
    {
      
        public Person(string name, string lastname, int avg)
        {
            this.name = name;
            this.lastname = lastname;
            Avg = avg;
        }

        public string name { get; set; }
        public string lastname { get; set; }
        public int Avg { get; set; }

   
}
}
