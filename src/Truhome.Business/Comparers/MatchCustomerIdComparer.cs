using Truhome.Business.Models.Response;

namespace Truhome.Business.Comparers;

public class MatchCustomerIdComparer : IEqualityComparer<Match>
{
    public bool Equals(Match x, Match y)
    {
        return x?.Fields?.CustomerId == y?.Fields?.CustomerId;
    }

    public int GetHashCode(Match obj)
    {
        return obj?.Fields?.CustomerId.GetHashCode() ?? 0;
    }
}