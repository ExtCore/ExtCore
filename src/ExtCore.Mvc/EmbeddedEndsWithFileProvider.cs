using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Primitives;

namespace ExtCore.Mvc
{
    public class EmbeddedEndsWithFileProvider : IFileProvider
    {
        private readonly Assembly assembly;
        private readonly IFileProvider fileProvider;
        private readonly DateTimeOffset lastModified;

        public EmbeddedEndsWithFileProvider(Assembly assembly)
        {
            this.assembly = assembly;
            this.fileProvider = new EmbeddedFileProvider(assembly);
            this.lastModified = DateTimeOffset.UtcNow;
        }

        public EmbeddedEndsWithFileProvider(Assembly assembly, IFileProvider fileProvider)
        {
            this.assembly = assembly;
            this.fileProvider = fileProvider;
            this.lastModified = DateTimeOffset.UtcNow;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            IFileInfo fileInfo = fileProvider.GetFileInfo(subpath);

            if (fileInfo is NotFoundFileInfo)
            {
                fileInfo = GetFileInfoEndsWith(subpath);
            }

            return fileInfo;
        }

        private IFileInfo GetFileInfoEndsWith(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            string name = Path.GetFileName(subpath);
            subpath = subpath.Replace("\\", ".").Replace("/", ".");
            string resourceNameMatch = assembly.GetManifestResourceNames().LastOrDefault(resourceName => resourceName.EndsWith(subpath));

            if (string.IsNullOrEmpty(resourceNameMatch))
            {
                return new NotFoundFileInfo(name);
            }

            return new EmbeddedResourceFileInfo(this.assembly, resourceNameMatch, name, this.lastModified);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return fileProvider.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return fileProvider.Watch(filter);
        }
    }
}
