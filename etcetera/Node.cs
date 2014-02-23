namespace etcetera
{
    using System;
    using System.Collections.Generic;

    public class Node
    {
        public int CreatedIndex { get; set; }
        public string Key { get; set; }
        public int ModifiedIndex { get; set; }
        public string Value { get; set; }
        public int? Ttl { get; set; }
        public DateTime? Expiration { get; set; }
        public List<Node> Nodes { get; set; }
        public bool Dir { get; set; }
    }
}