namespace aws_s3_file_validator.Models
{
    public class Dog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateOnly DateOfDeath { get; set; }
        public int Age { get; set; }
        public bool IsDead { get; set; }

        public Dog(string model)
        {
            string[] values = model.Split(',');

            Id = Guid.Parse(values[0]);
            Name = values[1];
            DateOfBirth = DateOnly.FromDateTime(DateTime.Parse(values[2]));
            DateOfDeath = DateOnly.FromDateTime(DateTime.Parse(values[3]));
            Age = int.Parse(values[4]);
            IsDead = bool.Parse(values[5]);
        }
    }
}