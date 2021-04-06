namespace SimpleShop
{
    public enum KeywordTypes
    {
        String,
        Int,
        Decimal
    }
    
    public struct Keyword
    {
        private string keyword;
        private KeywordTypes type;

        public Keyword(string keyword, KeywordTypes Type = KeywordTypes.String)
        {
            this.keyword = keyword;
            this.type = Type;
        }

        public string GetStart()
        {
            return "<" + this.keyword + ">";
        }

        public string GetEnd()
        {
            return "</" + this.keyword + ">";
        }

        public string GetString()
        {
            return this.keyword;
        }

        public KeywordTypes WhichType()
        {
            return this.type;
        }
    }

    public struct KeywordPair
    {
        public Keyword Key;
        public string Value;

        public KeywordPair(Keyword key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}   // namespace SimpleShop