namespace InterviewBC.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public override string ToString()
        {
            return $"{Id}-{Name}";
        }
    }

    public class PokemonResult
    {
        public int count { get; set; }
        public string? next { get; set; }
        public string? previous { get; set; }
        public List<Pokemon> results { get; set; }
    }

    public class Pokemon
    {
        public string? name { get; set; }
    }
}
