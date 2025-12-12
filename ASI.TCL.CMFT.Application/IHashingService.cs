namespace ASI.TCL.CMFT.Application
{
    public interface IHashingService
    {
        string Hash(string input);
        bool Verify(string input, string hash);
    }
}
