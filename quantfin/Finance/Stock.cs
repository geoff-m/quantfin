namespace Saturday.Finance;

public record Stock(string Symbol, string Exchange) : IComparable<Stock>
{
    public int CompareTo(Stock? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var symbolComparison = string.Compare(Symbol, other.Symbol, StringComparison.Ordinal);
        if (symbolComparison != 0) return symbolComparison;
        return string.Compare(Exchange, other.Exchange, StringComparison.Ordinal);
    }

    public virtual bool Equals(Stock? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Symbol == other.Symbol && Exchange == other.Exchange;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Symbol, Exchange);
    }
}