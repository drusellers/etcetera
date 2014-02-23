namespace etcetera
{
    using System;

    public static class UriHelpers
    {
        public static Uri AppendPath(this Uri uri, string path)
        {
            var path1 = uri.AbsolutePath.TrimEnd(new[]
            {
                '/'
            }) + "/" + path;
            return new UriBuilder(uri.Scheme, uri.Host, uri.Port, path1, uri.Query).Uri;
        }
    }
}