namespace Truhome.Business.Exceptions;

public static class TruhomeExceptions
{
    public static TruhomeException TE401 => new TruhomeException("TE401", "Unauthorized.");

    public static TruhomeException TE400 => new TruhomeException("TE400", "Missing required header: x-origin-system");

    public static TruhomeException TE403 => new TruhomeException("TE403", "Forbidden.");

    public static TruhomeException TE404(string entity) => new TruhomeException("TE404", string.Format("{0} not found.", entity));
}
