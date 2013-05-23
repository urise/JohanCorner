namespace CommonClasses.Helpers
{
    public interface IMethodData
    {
        string FullSignature { get; }
        string ReturnType { get; }
        string MethodName { get; }
        string Parameters { get; }
        bool IsValid { get; }
    }
}