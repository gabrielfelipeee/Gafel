using Sqids;

namespace CommonTestUtilities.IdObfuscation;

public class IdObfuscationBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new SqidsOptions()
        {
            MinLength = 10,
            Alphabet = "MBihxtFpvSzAGVXHgeTnmOwrLCof9Q4RbWuZaqdP0723DEI1s8cNY6Ul5KJyjk"
        });
    }
}

