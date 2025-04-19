namespace Infrastructure.Provider.Deepseek.Model;

public class DsChoice
{
    public int Index { get; set; }

    public DsMessage Message { get; set; }

    public string FinishReason { get; set; }
}