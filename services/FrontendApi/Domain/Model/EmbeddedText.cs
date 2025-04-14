namespace Domain.Model
{
    public class EmbeddedText
    {
        public String text { get; set; }

        public List<float> vector { get; set; }
    }
}