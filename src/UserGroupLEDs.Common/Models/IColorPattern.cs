namespace UserGroupLEDs.Common.Models;

public interface IColorPattern
{
    void Execute(Settings settings, AbortRequest abortRequest);
}