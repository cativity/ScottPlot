namespace ScottPlot.Testing;

/// <summary>
///     Helper class to detect for duplicate items in complex collections
///     and display helpful error messages to the console the facilitate debugging.
/// </summary>
public class DuplicateIdentifier<T>(string title)
{
    private readonly List<KeyValuePair<string, T>> _things = [];

    public void Add(string id, T thing)
    {
        _things.Add(new KeyValuePair<string, T>(id, thing));
    }

    public void ShouldHaveNoDuplicates()
    {
        HashSet<string> duplicateIDs = [];
        HashSet<string> seenIDs = [];

        foreach (string id in _things.Select(t => t.Key))
        {
            if (seenIDs.Contains(id))
            {
                duplicateIDs.Add(id);
            }

            seenIDs.Add(id);
        }

        if (duplicateIDs.Count == 0)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (string id in duplicateIDs)
        {
            IEnumerable<T> duplicateThings = _things.Where(x => x.Key == id).Select(x => x.Value);
            string duplicateThingsString = string.Join(", ", duplicateThings);

            sb.Append("The ")
              .Append(title)
              .Append(" \"")
              .Append(id)
              .Append("\" is not unique as it is shared by: ")
              .AppendLine(duplicateThingsString);
        }

        throw new InvalidOperationException(sb.ToString());
    }
}
